using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Guardian.Model;
using System.Collections.Generic;

namespace Guardian
{
    public static class Function1
    {
        [FunctionName("SearchTheGuardian")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            // parse query parameter
            string searchTerm = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "searchTerm", true) == 0)
                .Value;
            
            if (searchTerm == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a searchTerm on the query string.");
            }

            GuardianData guardianData = new GuardianData(searchTerm);
            await guardianData.Search();

            IEnumerable<GuardianGroupedData> guardianGroupedData = guardianData.GroupData();
            return req.CreateResponse(HttpStatusCode.OK, guardianGroupedData);
        }        
    }
}
