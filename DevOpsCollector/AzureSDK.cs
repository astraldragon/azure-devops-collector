using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DevOpsCollector.Models;

namespace DevOpsCollector
{
    public class AzureSDK : IAzureSDK
    {
        public string AccessToken { get; set; }
        public string BaseUrl { get; set; }
        public string ApiVersion { get; set; }
        private HttpClient _httpClient;

        public AzureSDK(string accessToken) {
            AccessToken = accessToken;
            BaseUrl = "https://dev.azure.com";
            ApiVersion = "6.1-preview.4";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(AccessToken))));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        public AzureSDK(string accessToken, HttpClient httpClient) {
            AccessToken = accessToken;
            BaseUrl = "https://dev.azure.com";
            ApiVersion = "6.1-preview.4";
            _httpClient = httpClient;
        }

        public IObservable<Project> StreamProjects(string organization) {
            return Observable.Create<IList<Project>>(async (obs, ct) => {
                string continuationToken = "";

                while(!ct.IsCancellationRequested) {
                    var pagedResponse = await GetProjectsAsync(organization, continuationToken);
                    continuationToken = pagedResponse.ContinuationToken;
                    obs.OnNext(pagedResponse.Response.Projects);
                    if (!pagedResponse.hasNextPage()) {
                        obs.OnCompleted();
                        break;
                    }
                }
            })
            .SelectMany(page => page);
        }

        public IObservable<PaginatedResponse<ProjectListResponse>> GetProjectsAsync(string organization, string continuationToken = "") {
            var response = _httpClient.GetAsync(String.Format("{0}/{1}/_apis/projects?api-version={2}", BaseUrl, organization, ApiVersion));
            return CreatePagedObservable<ProjectListResponse>(response);
        }

        private IObservable<PaginatedResponse<T>> CreatePagedObservable<T>(Task<HttpResponseMessage> httpResponseMessage) {
            return Observable.CombineLatest<T, HttpResponseHeaders, PaginatedResponse<T>>(
                httpResponseMessage.Result.Content.ReadAsAsync<T>().ToObservable(),
                Observable.Return(httpResponseMessage.Result.Headers),
                (x, y) => {
                    return new PaginatedResponse<T> {
                        Response = x,
                        ContinuationToken = y.GetValues("x-ms-continuationtoken").First()
                    };
                }
            );
        }
    }
}