﻿using MatthewDotCare.XStatic.Actions;
using MatthewDotCare.XStatic.Actions.Db;
using MatthewDotCare.XStatic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace MatthewDotCare.XStatic.Controllers
{
    [PluginController("xstatic")]
    public class ActionsController : UmbracoAuthorizedJsonController
    {
        private readonly IActionRepository _repo;

        public ActionsController(IActionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ActionModel> GetPostActions()
        {
            var actions = _repo.GetAllInCategory(ActionConstants.PostGenerationCategory);

            return actions.Select(a => new ActionModel(a));
        }

        [HttpPost]
        public ActionModel CreatePostAction([FromBody] ActionUpdateModel model)
        {
            var dataModel = new ActionDataModel
            {
                Name = model.Name,
                Category = ActionConstants.PostGenerationCategory,
                Config = model.Config,
                Type = model.Type
            };

            var entity = _repo.Create(dataModel);

            return new ActionModel(entity);
        }

        [HttpPost]
        public ActionModel UpdatePostAction([FromBody] ActionUpdateModel model)
        {
            var dataModel = new ActionDataModel
            {
                Id = model.Id,
                Name = model.Name,
                Category = ActionConstants.PostGenerationCategory,
                Config = model.Config,
                Type = model.Type
            };

            var entity = _repo.Update(dataModel);

            return new ActionModel(entity);
        }

        [HttpDelete]
        public void DeletePostAction(int id)
        {
            _repo.Delete(id);
        }
    }
}