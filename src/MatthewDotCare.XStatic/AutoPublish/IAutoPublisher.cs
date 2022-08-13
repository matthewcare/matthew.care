using Umbraco.Cms.Core.Models;

namespace MatthewDotCare.XStatic.AutoPublish
{
    public interface IAutoPublisher
    {
        void RunAutoPublish(IEnumerable<IContent> publishedEntities);
    }
}