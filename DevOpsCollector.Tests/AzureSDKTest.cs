using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Net.Http.Headers;

namespace DevOpsCollector.Tests
{
    public class AzureSDKTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""count"": 1, value: [] }"),
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.Content.Headers.Add("x-ms-continuationtoken", "abc");
            response.Headers.Add("x-ms-continuationtoken", "abc");
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var azureSDK = new AzureSDK("abc123", httpClient);

            azureSDK.GetProjectsAsync(organization: "testOrg").Subscribe(x => {
                Console.WriteLine(x.ToString());
            });

            Assert.Pass();
        }
    }
}