using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Services.Specification;
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

            ArgumentNullException.ThrowIfNullOrEmpty(basket.PaymentIntentId);
            var OrderRepo= _unitOfWork.GetRepository<Order,Guid>();
            var Spec= new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var ExistOrder= await OrderRepo.GetByIdAsync(Spec);
            if(ExistOrder is not null)
            {
                OrderRepo.Remove(ExistOrder);
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
                Subtotal = subtotal,
                PaymentIntentId = basket.PaymentIntentId
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

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email)
        {
            var Spec=new OrderSpecification(Email);
            var Orders = await _unitOfWork.GetRepository<Order, Guid>()
                .GetAllAsync(Spec);
            if(!Orders.Any())
                {
                return Error.NotFound("No orders found for this user!");
            }
            var orderDTOs = _mapper.Map<IEnumerable<OrderToReturnDTO>>(Orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(orderDTOs);
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if(!deliveryMethods.Any())
            {
                return Error.NotFound("No delivery methods found!");

            }
            var deliveryMethodDTOs = _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(deliveryMethodDTOs);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string Email)
        {
            var Spec=new OrderSpecification(id,Email);
            var order =  await _unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(Spec);
            if(order is null)
                {
                return Error.NotFound("Order not found!");
            }
            return _mapper.Map<OrderToReturnDTO>(order);
        }
    }
}
