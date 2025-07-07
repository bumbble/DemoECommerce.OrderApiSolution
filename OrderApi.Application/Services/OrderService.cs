using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        // GET PRODUCT
        public async Task<ProductDTO> GetProduct(int productId)
        {
            // Call ProductAPI using HTTP client
            // Redirection this call to the API Gateway, since product Api is not response to the outsiders
            var getProduct = await httpClient.GetAsync($"api/products/{productId}");

            if (!getProduct.IsSuccessStatusCode)
                return null!;

            // Deserialize the response to ProductDTO
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();

            return product ?? throw new Exception("Product not found");
        }

        // GET USER
        public async Task<AppUserDTO> GetUser(int userId)
        {
            // Call UserAPI using HTTP client
            // Redirection this call to the API Gateway, since user Api is not response to the outsiders
            var getUser = await httpClient.GetAsync($"api/users/{userId}");

            if (!getUser.IsSuccessStatusCode)
                return null!;

            // Deserialize the response to UserDTO
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user ?? throw new Exception("User not found");
        }

        // GET ORDER DETAILS BY ID
        public async Task<OrderDetailsDTO> GetOrderDetailsByOrderId(int orderId)
        {
            // Prepare Order 
            var order = await orderInterface.FindByIdAsync(orderId) ?? throw new Exception("Order not found");

            // Get retry policy from the resilience pipeline
            var retryPolicy = resiliencePipeline.GetPipeline("my-retry-pipeline");

            // Prepare Product - Execute the HTTP call with retry policy
            var productDTO = await retryPolicy.ExecuteAsync(async token => await GetProduct(order.ProductId));

            // Prepare Client - Execute the HTTP call with retry policy
            var appUserDTO = await retryPolicy.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDetailsDTO(
                order.Id,
                order.ProductId,
                order.ClientId,
                appUserDTO.Name,
                appUserDTO.Id,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                order.PurchaseQuantity * productDTO.Price,
                order.OrderDate
            );
        }

        // GET ORDERS BY CLIENT ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetByAsync(o => o.ClientId == clientId) ?? throw new Exception("Orders not found");

            // Convert to DTOs
            var orderDTOs = orders.ToDTOs();

            return orderDTOs;
        }
    }
}
