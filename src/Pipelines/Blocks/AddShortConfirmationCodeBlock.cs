using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;

namespace ShortConfirmationCodes.Pipelines.Blocks
{
 
    [PipelineDisplayName("AddShortConfirmationCode.Block")]
    public class AddShortConfirmationCodeBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commander;


        public AddShortConfirmationCodeBlock(CommerceCommander commander)
        {
            _commander = commander;
        }

        public override async Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            arg.OrderConfirmationId = await _commander.Command<CreateShortConfirmationCode>().Process(context.CommerceContext);
            return arg;
        }
    }
}