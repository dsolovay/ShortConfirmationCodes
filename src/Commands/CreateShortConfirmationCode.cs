using System;
using System.Text;
using System.Threading.Tasks;
using ShortConfirmationCodes.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;

namespace ShortConfirmationCodes.Pipelines.Blocks
{
  
    public class CreateShortConfirmationCode : CommerceCommand
    {
        private readonly CommerceCommander _commander;

        private static Random _random = new Random();
        private static object _myLock = new object();

        public CreateShortConfirmationCode(CommerceCommander commander, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _commander = commander;
        }

        public async Task<string> Process(CommerceContext commerceContext)
        {
            var policy = commerceContext.GetPolicy<ShortConfirmationCodePolicy>();
            int count = 0;
             
            string result = Guid.NewGuid().ToString("N").ToUpper(); //Fallback value.
          
            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                await this.PerformTransaction(commerceContext, async () =>
                 {
                     do
                     {
                         string value = CreateValue(policy);
                         string id = $"Entity-ShortConfirmationCode-{value}";
                         if (await NotYetUsed(commerceContext, id))
                         {
                             await ReserveIt(commerceContext, id);
                             result = value;
                             break;
                         }
                         count++;
                     } while (count <= policy.MaximumRetries);
                 });
                return result;
            }
        }

        private async Task ReserveIt(CommerceContext commerceContext, string id)
        {
            var entity= await _commander.GetEntity<CommerceEntity>(commerceContext, id, autoCreate: true);
            entity.Id = id;
            await _commander.PersistEntity(commerceContext, entity);
        }

        private async Task<bool> NotYetUsed(CommerceContext commerceContext, string id)
        {
            return await _commander.GetEntity<CommerceEntity>(commerceContext,id, autoCreate:false) == null;
        }

        private string CreateValue(ShortConfirmationCodePolicy policy)
        {
            var sb = new StringBuilder();
            string policyAllowedCharacters = policy.AllowedCharacters;
            int max = policyAllowedCharacters.Length-1;
            for (int i = 0; i < policy.CodeLength; i++)
            {
                
                sb.Append(policyAllowedCharacters[GetNextRandom(max)]);
            }
            
            return sb.ToString();
        }


        // Based on John Skeet's StaticRandom class.
        private static int GetNextRandom(int max)
        {
            lock (_myLock)
                return _random.Next(max);
        }
    }
}