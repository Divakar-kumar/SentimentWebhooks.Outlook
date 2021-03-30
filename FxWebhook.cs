using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessWebhooks.Github.Enums;

namespace ServerlessWebhooks.Github
{
    public static class FxWebhook
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> GetOutlookCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            Reply sentimentCategory=Reply.Later;

            log.LogInformation("Sentiment Webhook http function triggered");

            double sentimentScore = double.Parse(await new StreamReader(req.Body).ReadToEndAsync());

            if (sentimentScore < 0.4)
            {
                sentimentCategory = Reply.ASAP;
            }
            else if (sentimentScore < 0.7)
            {
                sentimentCategory = Reply.Normal;
            }

            log.LogInformation($"Sentiment category based on score { sentimentCategory }");

            return new OkObjectResult(sentimentCategory);

        }
    }
}

