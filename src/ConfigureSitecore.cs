using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ShortConfirmationCodes.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace ShortConfirmationCodes
{
   
    public class ConfigureSitecore : IConfigureSitecore
    {

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IOrderPlacedPipeline>(c =>
                    c.Replace<OrderPlacedAssignConfirmationIdBlock, AddShortConfirmationCodeBlock>()));

            services.RegisterAllCommands(assembly);
        }
    }
}