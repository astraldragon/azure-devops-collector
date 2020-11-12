using System.Collections.Generic;

namespace DevOpsCollector
{
    public class OrganizationResult
    {
        public string Organization { get; set; }
        public IList<string> Projects { get; set; }
        public IList<string> TestPlans { get; set; }
        public IList<string> TestSuites { get; set; }
        public IList<string> TestCases { get; set; }
    }
}