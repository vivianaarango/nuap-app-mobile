namespace nuap.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Models;
    using Plugin.Connectivity;
    using System.Text;
    using System.Net.Http.Headers;

    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se ha podido conectar a internet.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "67.205.184.22");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se ha podido conectar a internet.",
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
            string password
        ) {
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

        public async Task<TicketMessageResponse> GetTicketMessages(
            string accessToken,
            string urlBase,
            int ticketId
        ) {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync("/api/users/ticket/"+ticketId+"/messages");
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TicketMessageResponse>(resultJSON);
      
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TicketResponse> GetTickets(
            string accessToken,
            string urlBase
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync("/api/users/ticket");
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TicketResponse>(resultJSON);

                return result;
            }
            catch 
            {
                return null;
            }
        }

        public async Task<ReplyTicketResponse> ReplyTicket(
           string accessToken,
           string urlBase,
           int ticketID,
           string message
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("/api/users/ticket/reply",
                    new StringContent(string.Format(
                    "ticket_id={0}&message={1}",
                    ticketID, message),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReplyTicketResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ReplyTicketResponse> CreateTicket(
            string accessToken,
            string urlBase,
            string issues,
            string message
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("/api/users/ticket/create",
                    new StringContent(string.Format(
                    "issues={0}&message={1}",
                    issues, message),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReplyTicketResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProfileUserResponse> GetUserProfile(
            string accessToken,
            string urlBase
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync("/api/users/profile");
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProfileUserResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ReplyTicketResponse> EditProfile(
           string accessToken,
           string urlBase,
           string name,
           string email,
           string phone
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("/api/users/profile/edit",
                    new StringContent(string.Format(
                    "name={0}&email={1}&phone={2}",
                    name, email, phone),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReplyTicketResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}