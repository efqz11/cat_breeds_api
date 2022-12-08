using Newtonsoft.Json;
using System.Text.Json.Serialization;
using UI.App.Models;

namespace UI.App.Service
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient client;
        private System.Net.HttpStatusCode ResponseStatusCode;
        private Response400 response400;
        private Response403 response403; //Forbidden(403) or 401 unauthorized

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://housing-api.stag.mpao.mv/");
        }

        public async Task<string> TryLogin(string email, string password)
        {
            var response = await client.PostAsJsonAsync("auth/signin", new { email, password });
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResponseToken>(await response.Content.ReadAsStringAsync()).access_token;

            await HandleFailureResponse(response);
            throw new Exception(); // handle error response
        }

        public async Task<List<ResponseEmployment>> SearchById(string token, string id)
        {
            string path = DateTime.Now.Millisecond.ToString();
            var url = $"employments/{id}/{path}";

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<ResponseEmployment>>(await response.Content.ReadAsStringAsync());

            await HandleFailureResponse(response);
            throw new Exception(); // handle error response
        }

        public IEnumerable<string> GetErrors()
        {
            if (ResponseStatusCode == System.Net.HttpStatusCode.BadRequest)
                return response400.message;
            else return new[] { response403.message };  // both 401 & 403
        }

        private async Task HandleFailureResponse(HttpResponseMessage response)
        {
            ResponseStatusCode = response.StatusCode;
            if (ResponseStatusCode == System.Net.HttpStatusCode.BadRequest)
                response400 = JsonConvert.DeserializeObject<Response400>(await response.Content.ReadAsStringAsync());
            else
                response403 = JsonConvert.DeserializeObject<Response403>(await response.Content.ReadAsStringAsync());
        }
    }
}
