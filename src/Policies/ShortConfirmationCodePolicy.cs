using Sitecore.Commerce.Core;

namespace ShortConfirmationCodes.Policies
{
    public class ShortConfirmationCodePolicy : Policy
    {
        public int MaximumRetries { get; set; } = 3;
        public int CodeLength { get; set; } = 6;
        public string AllowedCharacters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}