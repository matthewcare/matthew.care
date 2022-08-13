using MatthewDotCare.XStatic.Actions;
using MatthewDotCare.XStatic.Deploy;
using MatthewDotCare.XStatic.Generator;
using MatthewDotCare.XStatic.Generator.Db;
using MatthewDotCare.XStatic.Generator.ExportTypes;
using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Generator.Transformers;
using MatthewDotCare.XStatic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace MatthewDotCare.XStatic.Controllers
{
    [PluginController("xstatic")]
    public class XStaticConfigController : UmbracoAuthorizedJsonController
    {
        private readonly IDeployerService _deployerService;
        private readonly IExportTypeService _exportTypeService;
        private readonly IExportTypeRepository _repo;
        private readonly GeneratorList _generatorList;
        private readonly TransformerList _transformerList;
        private readonly FileNameGeneratorList _fileNameGeneratorList;
        private readonly PostGenerationActionsList _postGenerationActionsList;

        public XStaticConfigController(IDeployerService deployerService,
            IExportTypeService exportTypeService,
            IExportTypeRepository repo,
            GeneratorList generatorList,
            TransformerList transformerList,
            FileNameGeneratorList fileNameGeneratorList,
            PostGenerationActionsList postGenerationActionsList)
        {
            _deployerService = deployerService;
            _exportTypeService = exportTypeService;
            _repo = repo;
            _generatorList = generatorList;
            _transformerList = transformerList;
            _fileNameGeneratorList = fileNameGeneratorList;
            _postGenerationActionsList = postGenerationActionsList;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<XStaticConfig> Get()
        {
            var deployers = _deployerService.GetDefinitions();
            var exportTypes = _exportTypeService.GetExportTypes();

            return new XStaticConfig
            {
                Deployers = deployers.Select(d => new DeployerModel(d)),
                ExportTypes = exportTypes.Select(e => new ExportTypeModel(e)),
                Generators = _generatorList.Generators.Select(g => new TypeModel(g)).ToList(),
                TransformerFactories = _transformerList.TransformerListFactories.Select(g => new TypeModel(g)).ToList(),
                FileNameGenerators = _fileNameGeneratorList.FileNameGenerators.Select(g => new TypeModel(g)).ToList(),
                PostGenerationActions = _postGenerationActionsList.PostActions.Select(g => new ConfigurableTypeModel(g)).ToList()
            };
        }

        [HttpPost]
        public ExportTypeModel CreateExportType([FromBody] ExportTypeUpdateModel model)
        {
            var dataModel = new ExportTypeDataModel
            {
                Name = model.Name,
                Generator = model.Generator,
                TransformerFactory = model.TransformerFactory,
                FileNameGenerator = model.FileNameGenerator
            };

            var entity = _repo.Create(dataModel);

            return new ExportTypeModel(entity);
        }

        [HttpPost]
        public ExportTypeModel UpdateExportType([FromBody] ExportTypeUpdateModel model)
        {
            var dataModel = new ExportTypeDataModel
            {
                Id = model.Id,
                Name = model.Name,
                Generator = model.Generator,
                TransformerFactory = model.TransformerFactory,
                FileNameGenerator = model.FileNameGenerator
            };

            var entity = _repo.Update(dataModel);

            return new ExportTypeModel(entity);
        }

        [HttpDelete]
        public void DeleteExportType(int id)
        {
            _repo.Delete(id);
        }
    }
}
