namespace nuap.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Models;
    using Plugin.Connectivity;
    using System.Text;

    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se ha podido conectar a internet1.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "67.205.184.22");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se ha podido conectar a internet2.",
                };
            }

            return new Response
            {
                IsSuccess = true,
            };
        }

        public async Task<LoginResponse> GetUser(
            string urlBase,
            string email,
            string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("/api/users/init",
                    new StringContent(string.Format(
                    "email={0}&password={1}",
                    email, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoginResponse>(resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}