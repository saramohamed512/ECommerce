using ECommerce.ServiceAbstraction;
using ECommerce.Shared.DTOS.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {

        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Result = await _orderService.CreateOrderAsync(orderDTO, GetEmailFromToken());
            return HandleResult(Result);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var Result = await _orderService.GetAllOrdersAsync(GetEmailFromToken());
            return HandleResult(Result);


        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var Result = await _orderService.GetOrderByIdAsync(id, GetEmailFromToken());
            return HandleResult(Result);
        }
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var Result = await _orderService.GetDeliveryMethods();
            return HandleResult(Result);
        }
    }
}
