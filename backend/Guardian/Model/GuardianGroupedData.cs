using System.Collections.Generic;

namespace Guardian.Model
{
    public class GuardianGroupedData
    {
        public string Section { get; set; }
        public IEnumerable<Result> Results { get; set; }
    }
}
