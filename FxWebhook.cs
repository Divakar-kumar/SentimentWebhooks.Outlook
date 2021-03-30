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
        [FunctionName("SentimentCategory")]
        public static async Task<IActionResult> GetSentimentCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            SentimentCategory sentimentCategory = SentimentCategory.Later;

            log.LogInformation("Sentiment Webhook http function triggered");

            double sentimentScore = double.Parse(await new StreamReader(req.Body).ReadToEndAsync());

            if (sentimentScore < 0.4)
            {
                sentimentCategory = SentimentCategory.ASAP;
            }
            else if (sentimentScore < 0.7)
            {
                sentimentCategory = SentimentCategory.Normal;
            }

            log.LogInformation($"Sentiment category based on score { sentimentCategory }");

            return new OkObjectResult(sentimentCategory);

        }
    }
}

