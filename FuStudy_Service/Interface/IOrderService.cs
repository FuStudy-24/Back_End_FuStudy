using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse> GetOrderByIdAsync(long id);
        Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
        Task<OrderResponse> UpdateOrderAsync(OrderRequest orderRequest, long orderId);
        Task<bool> DeleteOrderAsync(long orderId);
    }
}
