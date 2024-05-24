using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using CoreApiResponse;
using FuStudy_Service.Interface;
using FuStudy_Repository.Entity;
using FuStudy_Model.DTO.Response;
using FuStudy_Model.DTO.Request;
using Tools;
namespace FuStudy_API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return CustomResult("Data loaded!", orders);
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return CustomResult("Order not found", HttpStatusCode.NotFound);
            }

            return CustomResult("Data loaded!", order);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CustomResult(ModelState, HttpStatusCode.BadRequest);
                }

                var createdOrder = await _orderService.CreateOrderAsync(orderRequest);
                return CustomResult("Created successfully", createdOrder);
            }
            catch (CustomException.DataNotFoundException e)
            {
                return CustomResult(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception exception)
            {
                return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("UpdateOrder/{orderId}")]
        public async Task<IActionResult> UpdateOrder(long orderId, [FromBody] OrderRequest orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return CustomResult(ModelState, HttpStatusCode.BadRequest);
            }

            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(orderRequest, orderId);
                return CustomResult("Update successfully", updatedOrder);
            }
            catch (CustomException.DataNotFoundException e)
            {
                return CustomResult(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception exception)
            {
                return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteOrder/{orderId}")]
        public async Task<IActionResult> DeleteOrder(long orderId)
        {
            try
            {
                await _orderService.DeleteOrderAsync(orderId);
                return CustomResult("Delete order successfully", HttpStatusCode.OK);
            }
            catch (CustomException.DataNotFoundException e)
            {
                return CustomResult(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception exception)
            {
                return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
