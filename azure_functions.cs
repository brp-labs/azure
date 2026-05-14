/* Azure Functions - HTTP Trigger
 * 
 * .NET 8.0 Isolated Worker Model
 * Local runtime: Azure Functions Core Tools.
 * 
 * Test på localhost:
 * GET: http://localhost:7259/api/hello?name=Brian
 * GET: http://localhost:7259/api/code?code=12345   
 * 
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public class Functions
    {
        private readonly ILogger<Functions> _logger;

        public Functions(ILogger<Functions> logger)
        {
            _logger = logger;
        }


        // ========================================================================================
        [Function("HttpFunctionHello")]
        public IActionResult Hello([HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", "post",  // You can specify the HTTP methods you want to allow   
                Route = "hello")]  // You can specify a custom route here if needed | null
            HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            if (string.IsNullOrWhiteSpace(name))
                return new BadRequestObjectResult("Please pass a name in the query string");

            return new OkObjectResult($"Hello {name}, welcome to Azure Functions!");
        }

        // ========================================================================================
        [Function("HttpFunctionCode")]
        public IActionResult Code([HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", "post",  // You can specify the HTTP methods you want to allow   
                Route = "code")]  // You can specify a custom route here if needed | null
            HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string code = req.Query["code"];

            if (string.IsNullOrWhiteSpace(code))
                return new BadRequestObjectResult("Please pass a code in the query string");

            return new OkObjectResult($"Your code is {code}");
        }

        // ========================================================================================
        [Function("HttpFunctionSognekode")]   
        public IActionResult Sognekode([HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "sognekode")]
            HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string sognekode = req.Query["sognekode"];

            if (string.IsNullOrWhiteSpace(sognekode))
                return new BadRequestObjectResult("Please pass a sognekode in the query string");

            // return new OkObjectResult($"Your sognekode is {sognekode}");

            var dkTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")
            );


            var response = new
            {   metadata = new
                {
                    author = "Brian",
                    version = "1.0"
                },  
                data = new
                {
                    sognekode = sognekode,
                    sogn = "Beder Sogn",
                    status = "OK",
                    // timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    timestamp = dkTime.ToString("yyyy-MM-dd HH:mm:ss")
                }
            };

            return new JsonResult(response);
        }



    }
}
