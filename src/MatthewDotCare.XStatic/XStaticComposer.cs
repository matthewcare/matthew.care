using MatthewDotCare.XStatic.AutoPublish;
using MatthewDotCare.XStatic.Db;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace MatthewDotCare.XStatic
{
    public class XStaticComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, XStaticDatabaseNotificationHandler>();
            builder.AddNotificationHandler<ContentPublishingNotification, AutoPublishNotificationHandler>();
        }
    }
}