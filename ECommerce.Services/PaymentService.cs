using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.Orders;
using ECommerce.ServiceAbstraction;
using ECommerce.Services.Execptions;
using ECommerce.Shared.DTOS.BasketDTOs;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
       
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork=unitOfWork;
            _mapper = mapper;
            _configuration=configuration;

        }
        public async Task<BasketDTO?> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket == null)
            {
                throw new BasketNotFoundException(BasketId);
            }
            var Product = _unitOfWork.GetRepository<Domain.Entities.ProductModule.Product, int>();
            foreach (var item in Basket.Items)
            {
                var productEntity = await Product.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = productEntity.Price;
                var DeliveryMethod = await _unitOfWork.GetRepository<Domain.Entities.Orders.DeliveryMethod, int>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                Basket.ShippingPrice = DeliveryMethod.Price;
                var BasketAmount = (long)(Basket.Items.Sum(i => i.Quantity * i.Price) + DeliveryMethod.Price) * 100;
                var PaymentService = new PaymentIntentService();
                if (Basket.PaymentIntentId is null)
                {
                    var options = new PaymentIntentCreateOptions()
                    {
                        Amount = BasketAmount,
                        Currency = "USD",
                        PaymentMethodTypes = ["card"],
                    };
                    var intent = await PaymentService.CreateAsync(options);
                    Basket.PaymentIntentId = intent.Id;
                    Basket.ClentScreet = intent.ClientSecret;
                }
                else
                {
                    var options = new PaymentIntentUpdateOptions()
                    {
                        Amount = BasketAmount,
                    };
                    await PaymentService.UpdateAsync(Basket.PaymentIntentId, options);
                }
                await _basketRepository.CreateOrUpdateBasketAsync(Basket);
                return _mapper.Map<CustomerBasket, BasketDTO>(Basket);
            }
            // If the basket has no items, return null
            return null;
        }
    }
}
