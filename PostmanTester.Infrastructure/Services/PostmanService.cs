using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostmanTester.Application.Constants;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Interfaces.Services;
using PostmanTester.Application.Models.Responses.Postman;
using PostmanTester.Application.Models.Results;
using RestSharp;
using System.Net;
using Environment = PostmanTester.Application.Models.Responses.Postman.Environment;

namespace PostmanTester.Infrastructure.Services
{
    public class PostmanService : IPostmanService
    {
        private readonly IConfiguration _configuration;
        private readonly string PostmanBaseUrl;
        private readonly IUserService _userService;

        public PostmanService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            PostmanBaseUrl = _configuration.GetSection("PostmanSettings").GetValue<string>("BaseUrl");
            _userService = userService;
        }

        public class ApiRequest
        {
            public string Name { get; set; }
            public string Method { get; set; }
            public string Url { get; set; }
            public string? BodyMode { get; set; }
            public JObject? Body { get; set; }
        }

        private protected RestRequest CreateRestRequest(ApiRequest request)
        {
            try
            {
                var restRequest = new RestRequest();
                restRequest.Method = request.Method.ToUpper() switch
                {
                    "GET" => Method.Get,
                    "POST" => Method.Post,
                    "PUT" => Method.Put,
                    "DELETE" => Method.Delete,
                    _ => throw new NotSupportedException($"Method {request.Method} is not supported."),
                };
                return restRequest;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<RestResponse> ExecuteRawRequestAsync(ApiRequest request)
        {
            try
            {
                var client = new RestClient(request.Url);
                var restRequest = CreateRestRequest(request);
                restRequest.AddHeader("Authorization", "Bearer 9mvcX4Lb7GQzy1MzYEMCkLdcoiDy4DVAkvCBcmG82ESbAuV+vO2Y7+Ud48MLVW3ARuTjsYpZJem3WTRvfeYnNz4NfZ6yaOgIGj/tdq25skOVcbz+S4iADUN2u8ou87u7F/ggfJjiL1PWntpOZjfyeZIIdN1jeMhqPxMSsiWhSEBCAK4KPWs0BZ3X9qacTnwIj4mhJnJL0KRuL+ha4kZEKyxCTgETYFq4obYxvM7yDfayzNSySvyI/0ON5qbxO1QB60Hy6bFl12JUAiBO3u5cBhcDaa2KRGPNYKZ+h7g6N0wwN4TxZik2bNw8f0lASaV4+GeWDoYt+R7letROFK0O9/BF1/dVHTV0h3P5PU8fMD4V4gIVySUydrrjvZZqYN28xsQM73hnLdxyqrPmUfr8HMYmeHVQ6vRW8QtqLy25wONrWJkLpb4QirS9CtkMD6Hr/vvaiivd7m6GYwq1kpQZS1fg+8SrQRSviBLjUSDEoH0FM53IIvtWCXyIUoO7cFKlF+cvjUb7qRSkMFip3i5FgGTP6tHTXMtQUent5gkb/2ql05ZnSlNRU73gu8wFGPDYsTvH/VFI7DBKBy4nEjsKh28EGQD2aN6Z/7GIb+rUfBc=");
                if (request.Body != null)
                {
                    restRequest.AddJsonBody(request.Body.ToString());
                }
                var response = await client.ExecuteAsync(restRequest);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<RestResponse> ExecuteFormDataRequestAsync(ApiRequest request)
        {
            try
            {
                var client = new RestClient(request.Url);
                var restRequest = CreateRestRequest(request);
                if (request.Body != null)
                {
                    foreach (var field in request.Body["formdata"])
                    {
                        restRequest.AddParameter(field["key"].ToString(), field["value"].ToString());
                    }
                }
                return await client.ExecuteAsync(restRequest);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<RestResponse> ExecuteUrlEncodedRequestAsync(ApiRequest request)
        {
            try
            {
                var client = new RestClient(request.Url);
                var restRequest = CreateRestRequest(request);
                if (request.Body != null)
                {
                    foreach (var field in request.Body["urlencoded"])
                    {
                        restRequest.AddParameter(field["key"].ToString(), field["value"].ToString(), ParameterType.GetOrPost);
                    }
                }
                return await client.ExecuteAsync(restRequest);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IDictionary<string, HttpStatusCode>> RunTestAsync(CollectionDetail collectionDetail)
        {
            var serviceList = new Dictionary<string, HttpStatusCode>();
            var collections = collectionDetail.item.FirstOrDefault().item;
            foreach (var collection in collections)
            {
                var url = collection.request.url.host.FirstOrDefault();
                var baseUrl = collectionDetail.variable.Find(x => x.key == url.Replace("{{","").Replace("}}","")).value;

                var apiRequest = new ApiRequest
                {
                    Name = collection.name,
                    Method = collection.request?.method,
                    Url = "" + collection.request?.url?.raw.Replace(url, baseUrl),
                    BodyMode = collection.request?.body?.mode
                };

                if (!string.IsNullOrEmpty(apiRequest.BodyMode))
                {
                    if (apiRequest.BodyMode == "raw")
                    {
                        apiRequest.Body = JObject.Parse(collection.request.body.raw);
                    }
                    else if (apiRequest.BodyMode == "formdata")
                    {
                        //apiRequest.Body = new JObject { ["formdata"] = collection.request.body["formdata"]};
                    }
                    else if (apiRequest.BodyMode == "urlencoded")
                    {
                        apiRequest.Body = JObject.Parse(JsonConvert.SerializeObject(collection.request.body.urlencoded));
                    }
                }

                RestResponse response = apiRequest.BodyMode switch
                {
                    "raw" => await ExecuteRawRequestAsync(apiRequest),
                    "formdata" => await ExecuteFormDataRequestAsync(apiRequest),
                    "urlencoded" => await ExecuteUrlEncodedRequestAsync(apiRequest),
                    _ => await ExecuteRawRequestAsync(apiRequest),
                };

                serviceList.Add(apiRequest.Url, response.StatusCode);
            }
            return serviceList;
        }

        private async Task<IDataResult<T>> GetAsync<T>(string url)
        {
            var user = await _userService.GetByTokenAsync();
            if (user == null) return new ErrorDataResult<T>(default);
            if (!user.Success || user.Data == null) return new ErrorDataResult<T>(default, user.Message, user.MessageCode, user.StatusCode);

            var apiKey = user.Data.ApiKeys?.FirstOrDefault()?.Name;
            if (apiKey == null) return new ErrorDataResult<T>(default, Messages.DataNotFound);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(PostmanBaseUrl);
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var result = await client.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var jsonData = await result.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(jsonData);
                    return new SuccessDataResult<T>(data);
                }
                var errorResult = await result.Content.ReadAsStringAsync();
                return new ErrorDataResult<T>(default, errorResult);
            }
        }


        #region Workspaces
        public async Task<IDataResult<List<Workspace>>> GetListWorkspaces()
        {
            var result = await GetAsync<WorkspaceResponse>("workspaces");
            if (!result.Success) return new ErrorDataResult<List<Workspace>>(new(), result.Message, result.MessageCode, result.StatusCode);

            return new SuccessDataResult<List<Workspace>>(result.Data.workspaces, result.Message, result.MessageCode, result.StatusCode);
        }

        public async Task<IDataResult<Workspace>> GetWorkspace(string workspaceId)
        {
            var result = await GetAsync<WorkspaceDetailResponse>($"workspaces/{workspaceId}");
            if (!result.Success) return new ErrorDataResult<Workspace>(null, result.Message, result.MessageCode, result.StatusCode);

            return new SuccessDataResult<Workspace>(result.Data.workspace, result.Message, result.MessageCode, result.StatusCode);
        }
        #endregion

        #region Collections
        public async Task<IDataResult<List<Collection>>> GetListCollections()
        {
            var result = await GetAsync<CollectionResponse>("collections");
            if (!result.Success) return new ErrorDataResult<List<Collection>>(new(), result.Message, result.MessageCode, result.StatusCode);

            return new SuccessDataResult<List<Collection>>(result.Data.collections, result.Message, result.MessageCode, result.StatusCode);
        }

        public async Task<IDataResult<CollectionDetail>> GetCollection(string collectionId)
        {
            var result = await GetAsync<CollectionDetailResponse>($"collections/{collectionId}");
            if (!result.Success) return new ErrorDataResult<CollectionDetail>(null, result.Message, result.MessageCode, result.StatusCode);

            return new SuccessDataResult<CollectionDetail>(result.Data.collection, result.Message, result.MessageCode, result.StatusCode);
        }
        #endregion

        #region Environments
        public async Task<IDataResult<List<Environment>>> GetListEnvironments()
        {
            var result = await GetAsync<EnvironmentResponse>("environments");
            if (!result.Success) return new ErrorDataResult<List<Environment>>(new(), result.Message, result.MessageCode, result.StatusCode);

            return new SuccessDataResult<List<Environment>>(result.Data.environments, result.Message, result.MessageCode, result.StatusCode);
        }

        public async Task<IDataResult<Environment>> GetEnvironment(string environmentId)
        {
            var result = await GetAsync<EnvironmentDetailResponse>($"environments/{environmentId}");
            if (!result.Success) return new ErrorDataResult<Environment>(new(), result.Message, result.MessageCode, result.StatusCode);

            return new SuccessDataResult<Environment>(result.Data.environment, result.Message, result.MessageCode, result.StatusCode);
        }
        #endregion
    }
}
