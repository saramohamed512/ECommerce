using ECommerce.ServiceAbstraction;
using ECommerce.Shared.DTOS.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController: ControllerBase
    {
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService= basketService;
        }
        #region Get Basket by Id
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id)
        {
            var basket= await _basketService.GetBasketAsync(id);
            return Ok(basket);
        }
        #endregion
        #region Create or Update Basket
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket)
        {
            var updatedBasket= await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }
        #endregion
        #region Delete Basket
        [HttpDelete("{id}")]    
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var result= await _basketService.DeleteBasketAsync(id);
           return Ok(result);
        }
        #endregion
    }
}
