using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        public OrderService(IMapper mapper,IUnitOfWork unitOfWork ,IBasketRepository basketRepository) 
        {
            _mapper=mapper;
            _unitOfWork=unitOfWork;
            _basketRepository=basketRepository;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            var orderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);
            var basket =  await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if(basket is null)
            {
                return Error.NotFound("Basket not found!");
            }
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (Product is null)
                {
                    return Error.NotFound($"Product with id {item.Id} not found!");
                }
                orderItems.Add (CreateOrderItem(item, Product));
                
            }
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (deliveryMethod is null)
            {
                return Error.NotFound($"Delivery Method with id {orderDTO.DeliveryMethodId} not found!");
            }
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var order = new Order()
            {
                UserEmil = Email,
                Address = orderAddress,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                Subtotal = subtotal
            };
            await _unitOfWork.GetRepository<Order,Guid>().AddAsync(order);
            int Result = await _unitOfWork.SaveChangesAsync();
            if (Result == 0)
            {
                return Error.Failure("Failed to create order!");
            }
            return _mapper.Map<OrderToReturnDTO>(order);
             
        }

        private static OrderItem CreateOrderItem(Domain.Entities.BasketModule.BasketItem item, Product? Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered()
                {
                    ProductId = Product.Id,
                    ProductName = Product.Name,
                    PictureUrl = Product.PictureUrl
                },
                Price = Product.Price,
                Quantity = item.Quantity
            };
        }
    }
}
