using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            SentimentCategory sentimentCategory = SentimentCategory.LATER;

            log.LogInformation("Sentiment Webhook http function triggered");

            var sentimentObjects = JArray.Parse(await new StreamReader(req.Body).ReadToEndAsync());

            log.LogInformation($"Sentiment object array {sentimentObjects}");

            foreach (var sentiment in sentimentObjects)
            {
                
                var sentimentScore = double.Parse(Convert.ToString(sentiment["text"]));

                // If content of mail is negative , 
                // return ASAP as response for immediate attention

                if (sentimentScore < 0.4)
                {
                    sentimentCategory = SentimentCategory.ASAP;
                }

                // If content of mail is neutral , 
                // return NORMAL as response

                else if (sentimentScore < 0.7)
                {
                    sentimentCategory = SentimentCategory.NORMAL;
                }

                log.LogInformation($"Sentiment category based on score { sentimentCategory }");                
            }

            return new OkObjectResult(sentimentCategory.ToString());
        }
    }
}

