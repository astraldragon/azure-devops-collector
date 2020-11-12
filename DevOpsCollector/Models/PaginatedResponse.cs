using System;

namespace DevOpsCollector.Models
{
    public class PaginatedResponse<T>
    {
        public string ContinuationToken { get; set; }
        public T Response { get; set; }

        public bool hasNextPage() {
            return String.IsNullOrEmpty(this.ContinuationToken);
        }
    }
}