using System;
using DevOpsCollector.Models;

namespace DevOpsCollector
{
    public interface IAzureSDK
    {
        string AccessToken { get; set; }
        string BaseUrl { get; set; }
        string ApiVersion { get; set; }
        public IObservable<Project> StreamProjects(string organization);
        IObservable<PaginatedResponse<ProjectListResponse>> GetProjectsAsync(string organization, string continuationToken);
    }
}