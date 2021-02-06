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

        public async Task<ProfileCommerceResponse> GetCommerceProfile(
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
                var response = await client.GetAsync("/api/commerces/profile");
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProfileCommerceResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<CategoryResponse> GetCategories(
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
                var response = await client.GetAsync("/api/products/categories");
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CategoryResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProductResponse> GetProductsCommerce(
            string accessToken,
            string urlBase,
            int categoryID
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync("/api/products/category/"+categoryID);
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<OrderResponse> GetOrders(
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
                var response = await client.GetAsync("/api/users/orders");
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OrderResponse>(resultJSON);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ReplyTicketResponse> CreateProduct(
           string accessToken,
           string urlBase,
           int categoryID,
           string name,
           string sku, 
           string brand,
           string description,
           int stock,
           double weight,
           double height,
           double length,
           double width,
           double purchasePrice,
           double salePrice,
           double specialPrice,
           int hasSpecialPrice,
           int isFeatured
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("/api/products/commerce/create",
                    new StringContent(string.Format(
                    "category_id={0}&name={1}&sku={2}&brand={3}&description={4}&is_featured={5}&stock={6}&weight={7}&length={8}&width={9}&height={10}&purchase_price={11}&sale_price={12}&special_price={13}&has_special_price={14}",
                    categoryID, name, sku, brand, description, isFeatured, stock, weight, length, width, height, purchasePrice, salePrice, specialPrice, hasSpecialPrice),
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

        public async Task<ReplyTicketResponse> EditProduct(
           string accessToken,
           string urlBase,
           int categoryID,
           string name,
           string sku,
           string brand,
           string description,
           int stock,
           double weight,
           double height,
           double length,
           double width,
           double purchasePrice,
           double salePrice,
           double specialPrice,
           int hasSpecialPrice,
           int isFeatured,
           int productID
        )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("/api/products/commerce/create",
                    new StringContent(string.Format(
                    "has_special_price={0}&category_id={1}&name={2}&sku={3}&brand={4}&description={5}&is_featured={6}&stock={7}&weight={8}&length={9}&width={10}&height={11}&purchase_price={12}&sale_price={13}&special_price={14}&product_id={15}",
                    hasSpecialPrice, categoryID, name, sku, brand, description, isFeatured, stock, weight, length, width, height, purchasePrice, salePrice, specialPrice, productID),
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