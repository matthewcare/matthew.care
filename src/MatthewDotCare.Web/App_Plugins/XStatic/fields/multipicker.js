
/**
 * The controller that is used for a couple different Property Editors: Multi Node Tree Picker, Content Picker,
 * since this is used by MNTP and it supports content, media and members, there is code to deal with all 3 of those types
 * @param {any} $scope
 * @param {any} entityResource
 * @param {any} editorState
 * @param {any} iconHelper
 * @param {any} $routeParams
 * @param {any} angularHelper
 * @param {any} navigationService
 * @param {any} $location
 * @param {any} localizationService
 */
function contentPickerController($scope, entityResource, editorState, iconHelper, $routeParams, angularHelper, navigationService, $location, localizationService, editorService, $q) {

    var vm = {
        labels: {
            general_recycleBin: "",
            general_add: ""
        }
    };

    var unsubscribe;

    function subscribe() {
        unsubscribe = $scope.$on("formSubmitting", function (ev, args) {
            var currIds = _.map($scope.renderModel, function (i) {
                return $scope.property.config.idType === "udi" ? i.udi : i.id;
            });
            $scope.property.value = trim(currIds.join(), ",");
        });
    }

    function trim(str, chr) {
        var rgxtrim = (!chr) ? new RegExp('^\\s+|\\s+$', 'g') : new RegExp('^' + chr + '+|' + chr + '+$', 'g');
        return str.replace(rgxtrim, '');
    }

    function startWatch() {

        //due to the way angular-sortable works, it needs to update a model, we don't want it to update renderModel since renderModel
        //is updated based on changes to model.value so if we bound angular-sortable to that and put a watch on it we'd end up in a
        //infinite loop. Instead we have a custom array model for angular-sortable and we'll watch that which we'll use to sync the model.value
        //which in turn will sync the renderModel.
        $scope.$watchCollection("sortableModel", function (newVal, oldVal) {
            $scope.property.value = newVal.join();
        });

        //if the underlying model changes, update the view model, this ensures that the view is always consistent with the underlying
        //model if it changes (i.e. based on server updates, or if used in split view, etc...)
        $scope.$watch("property.value", function (newVal, oldVal) {
            if (newVal !== oldVal) {
                syncRenderModel(true);
            }
        });
    }

    $scope.renderModel = [];
    $scope.sortableModel = [];

    $scope.labels = vm.labels;

    $scope.dialogEditor = editorState && editorState.current && editorState.current.isDialogEditor === true;

    //the default pre-values
    var defaultConfig = {
        multiPicker: false,
        showOpenButton: false,
        showEditButton: false,
        showPathOnHover: false,
        dataTypeKey: null,
        maxNumber: 1,
        minNumber: 0,
        startNode: {
            query: "",
            type: "content",
            id: $scope.property.config.startNodeId ? $scope.property.config.startNodeId : -1 // get start node for simple Content Picker
        }
    };

    // sortable options
    $scope.sortableOptions = {
        axis: "y",
        containment: "parent",
        distance: 10,
        opacity: 0.7,
        tolerance: "pointer",
        scroll: true,
        zIndex: 6000,
        update: function (e, ui) {
            angularHelper.getCurrentForm($scope).$setDirty();
        }
    };

    if ($scope.property.config) {
        //special case, if the `startNode` is falsy on the server config delete it entirely so the default value is merged in
        if (!$scope.property.config.startNode) {
            delete $scope.property.config.startNode;
        }
        //merge the server config on top of the default config, then set the server config to use the result
        $scope.property.config = angular.extend(defaultConfig, $scope.property.config);

        // if the property is mandatory, set the minCount config to 1 (unless of course it is set to something already),
        // that way the minCount/maxCount validation handles the mandatory as well
        if ($scope.property.validation && $scope.property.validation.mandatory && !$scope.property.config.minNumber) {
            $scope.property.config.minNumber = 1;
        }
    }

    //Umbraco persists boolean for prevalues as "0" or "1" so we need to convert that!
    $scope.property.config.multiPicker = Object.toBoolean($scope.property.config.multiPicker);
    $scope.property.config.showOpenButton = Object.toBoolean($scope.property.config.showOpenButton);
    $scope.property.config.showEditButton = Object.toBoolean($scope.property.config.showEditButton);
    $scope.property.config.showPathOnHover = Object.toBoolean($scope.property.config.showPathOnHover);

    var entityType = $scope.property.config.startNode.type === "member"
        ? "Member"
        : $scope.property.config.startNode.type === "media"
            ? "Media"
            : "Document";
    $scope.allowOpenButton = entityType === "Document";
    $scope.allowEditButton = entityType === "Document";
    $scope.allowRemoveButton = true;

    //the dialog options for the picker
    var dialogOptions = {
        multiPicker: $scope.property.config.multiPicker,
        entityType: entityType,
        filterCssClass: "not-allowed not-published",
        startNodeId: null,
        dataTypeKey: $scope.property.dataTypeKey,
        currentNode: editorState ? editorState.current : null,
        callback: function (data) {
            if (angular.isArray(data)) {
                _.each(data, function (item, i) {
                    $scope.add(item);
                });
            } else {
                $scope.clear();
                $scope.add(data);
            }
            angularHelper.getCurrentForm($scope).$setDirty();
        },
        treeAlias: $scope.property.config.startNode.type,
        section: $scope.property.config.startNode.type,
        idType: "udi"
    };

    //since most of the pre-value config's are used in the dialog options (i.e. maxNumber, minNumber, etc...) we'll merge the
    // pre-value config on to the dialog options
    angular.extend(dialogOptions, $scope.property.config);

    dialogOptions.dataTypeKey = $scope.property.dataTypeKey;

    // if we can't pick more than one item, explicitly disable multiPicker in the dialog options
    if ($scope.property.config.maxNumber && parseInt($scope.property.config.maxNumber) === 1) {
        dialogOptions.multiPicker = false;
    }

    // add the current filter (if any) as title for the filtered out nodes
    if ($scope.property.config.filter) {
        localizationService.localize("contentPicker_allowedItemTypes", [$scope.property.config.filter]).then(function (data) {
            dialogOptions.filterTitle = data;
        });
    }

    if ($scope.property.config.startNode.query) {
        //if we have a query for the startnode, we will use that.
        var rootId = $routeParams.id;
        entityResource.getByQuery($scope.property.config.startNode.query, rootId, "Document").then(function (ent) {
            dialogOptions.startNodeId = ($scope.property.config.idType === "udi" ? ent.udi : ent.id).toString();
        });
    }
    else {
        dialogOptions.startNodeId = $scope.property.config.startNode.id;
    }

    console.log("config", $scope.property.config);
    console.log("config startNode", $scope.property.config.startNode);

    //dialog
    $scope.openCurrentPicker = function () {
        $scope.currentPicker = dialogOptions;

        console.log("openCurrentPicker", dialogOptions);

        $scope.currentPicker.submit = function (model) {
            if (angular.isArray(model.selection)) {
                _.each(model.selection, function (item, i) {
                    $scope.add(item);
                });
                angularHelper.getCurrentForm($scope).$setDirty();
            }
            angularHelper.getCurrentForm($scope).$setDirty();
            editorService.close();
        }

        $scope.currentPicker.close = function () {
            editorService.close();
        }

        //open the correct editor based on the entity type
        switch (entityType) {
            case "Document":
                editorService.contentPicker($scope.currentPicker);
                break;
            case "Media":
                console.log("Opening media picker");
                editorService.mediaPicker($scope.currentPicker);
                break;
            case "Member":
                editorService.memberPicker($scope.currentPicker);
                break;

            default:
        }

    };

    $scope.remove = function (index) {
        var currIds = $scope.property.value && $scope.property.value.split ? $scope.property.value.split(',') : [];
        if (currIds.length > 0) {
            currIds.splice(index, 1);
            angularHelper.getCurrentForm($scope).$setDirty();
            $scope.property.value = currIds.join();
        }
    };

    $scope.showNode = function (index) {
        var item = $scope.renderModel[index];
        var id = item.id;
        var section = $scope.property.config.startNode.type.toLowerCase();

        entityResource.getPath(id, entityType).then(function (path) {
            navigationService.changeSection(section);
            navigationService.showTree(section, {
                tree: section, path: path, forceReload: false, activate: true
            });
            var routePath = section + "/" + section + "/edit/" + id.toString();
            $location.path(routePath).search("");
        });
    }

    $scope.add = function (item) {
        var currIds = $scope.property.value ? $scope.property.value.split(',') : [];

        var itemId = ($scope.property.config.idType === "udi" ? item.udi : item.id).toString();

        if (currIds.indexOf(itemId) < 0) {
            currIds.push(itemId);
            $scope.property.value = currIds.join();
        }
    };

    $scope.clear = function () {
        $scope.property.value = null;
    };

    $scope.openContentEditor = function (node) {
        var contentEditor = {
            id: node.id,
            submit: function (model) {
                // update the node
                node.name = model.contentNode.name;
                node.published = model.contentNode.hasPublishedVersion;
                if (entityType !== "Member") {
                    entityResource.getUrl(model.contentNode.id, entityType).then(function (data) {
                        node.url = data;
                    });
                }
                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        };
        editorService.contentEditor(contentEditor);
    };

    //when the scope is destroyed we need to unsubscribe
    $scope.$on('$destroy', function () {
        if (unsubscribe) {
            unsubscribe();
        }
    });

    /** Syncs the renderModel based on the actual model.value and returns a promise */
    function syncRenderModel(doValidation) {

        var valueIds = $scope.property.value && $scope.property.value.split ? $scope.property.value.split(',') : [];

        //sync the sortable model
        $scope.sortableModel = valueIds;

        //load current data if anything selected
        if (valueIds.length > 0) {

            //need to determine which items we already have loaded
            var renderModelIds = _.map($scope.renderModel, function (d) {
                return ($scope.property.config.idType === "udi" ? d.udi : d.id).toString();
            });

            //get the ids that no longer exist
            var toRemove = _.difference(renderModelIds, valueIds);


            //remove the ones that no longer exist
            for (var j = 0; j < toRemove.length; j++) {
                var index = renderModelIds.indexOf(toRemove[j]);
                $scope.renderModel.splice(index, 1);
            }

            //get the ids that we need to lookup entities for
            var missingIds = _.difference(valueIds, renderModelIds);

            if (missingIds.length > 0) {
                return entityResource.getByIds(missingIds, entityType).then(function (data) {

                    _.each(valueIds,
                        function (id, i) {
                            var entity = _.find(data, function (d) {
                                return $scope.property.config.idType === "udi" ? (d.udi == id) : (d.id == id);
                            });

                            if (entity) {
                                addSelectedItem(entity);
                            }

                        });

                    setSortingState($scope.renderModel);
                    return $q.when(true);
                });
            }
            else {
                //if there's nothing missing, make sure it's sorted correctly

                var current = $scope.renderModel;
                $scope.renderModel = [];
                for (var k = 0; k < valueIds.length; k++) {
                    var id = valueIds[k];
                    var found = _.find(current, function (d) {
                        return $scope.property.config.idType === "udi" ? (d.udi == id) : (d.id == id);
                    });
                    if (found) {
                        $scope.renderModel.push(found);
                    }
                }

                setSortingState($scope.renderModel);
                return $q.when(true);
            }
        }
        else {
            $scope.renderModel = [];

            setSortingState($scope.renderModel);
            return $q.when(true);
        }

    }

    function setEntityUrl(entity) {

        // get url for content and media items
        if (entityType !== "Member") {
            entityResource.getUrl(entity.id, entityType).then(function (data) {
                // update url
                angular.forEach($scope.renderModel, function (item) {
                    if (item.id === entity.id) {
                        if (entity.trashed) {
                            item.url = vm.labels.general_recycleBin;
                        } else {
                            item.url = data;
                        }
                    }
                });
            });
        }

    }

    function addSelectedItem(item) {

        // set icon
        if (item.icon) {
            item.icon = iconHelper.convertFromLegacyIcon(item.icon);
        }

        // set default icon
        if (!item.icon) {
            switch (entityType) {
                case "Document":
                    item.icon = "icon-document";
                    break;
                case "Media":
                    item.icon = "icon-picture";
                    break;
            }
        }

        $scope.renderModel.push({
            "name": item.name,
            "id": item.id,
            "udi": item.udi,
            "icon": item.icon,
            "path": item.path,
            "url": item.url,
            "trashed": item.trashed,
            "published": (item.metaData && item.metaData.IsPublished === false && entityType === "Document") ? false : true
            // only content supports published/unpublished content so we set everything else to published so the UI looks correct
        });

        setEntityUrl(item);
    }

    function setSortingState(items) {
        // disable sorting if the list only consist of one item
        if (items.length > 1) {
            $scope.sortableOptions.disabled = false;
        } else {
            $scope.sortableOptions.disabled = true;
        }
    }

    function init() {
        localizationService.localizeMany(["general_recycleBin", "general_add"])
            .then(function (data) {
                vm.labels.general_recycleBin = data[0];
                vm.labels.general_add = data[1];

                syncRenderModel(false).then(function () {
                    //everything is loaded, start the watch on the model
                    startWatch();
                    subscribe();
                });
            });
    }

    init();

}

angular.module('umbraco').controller("multiPickerController", contentPickerController);
