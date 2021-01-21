using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Guardian.Model
{
    public class GuardianData
    {
        public Response Response { get; set; }
        private readonly string searchTerm;
        private string responseString;

        public GuardianData(string searchTerm)
        {
            this.searchTerm = searchTerm;
        }

        public async Task Search()
        {
            string guardianApiKey = ConfigurationManager.AppSettings["GuardianApiKey"],
                guardianApiUrl = ConfigurationManager.AppSettings["GuardianApiUrl"],
                guardianApiCompleteUrl = string.Format("{0}{1}&api-key={2}", guardianApiUrl, this.searchTerm, guardianApiKey);

            using (HttpClient client = new HttpClient())
            {
                this.responseString = await client.GetStringAsync(guardianApiCompleteUrl);
            }            
        }

        public IEnumerable<GuardianGroupedData> GroupData()
        {
            GuardianData result = JsonConvert.DeserializeObject<GuardianData>(this.responseString);

            var groupedBySectionId = result.Response.Results.GroupBy(gb => gb.SectionName)
                    .Select(group => new GuardianGroupedData
                    {
                        Section = group.Key,
                        Results = group.ToList()
                    }).ToList();

            return groupedBySectionId;
        } 
    }
}
