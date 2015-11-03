using FineBot.Common.Infrastructure;

namespace FineBot.API.FinesApi
{
    public class FineSecondedResult : ValidationResult
    {
        public FineSecondedResult(ValidationResult result)
        {
            this.Append(result);
        }

        public FineWithUserModel FineWithUserModel { get; set; }
    }
}