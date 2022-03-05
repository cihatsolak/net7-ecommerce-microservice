namespace CourseSales.Services.Discount.Controllers
{
    public class DiscountsController : BaseApiController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(
            IDiscountService discountService, 
            ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discountsResponseModel = await _discountService.GetAllAsync();
            return CreateActionResultInstance(discountsResponseModel);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discountResponseModel = await _discountService.GetByIdAsync(id);
            return CreateActionResultInstance(discountResponseModel);
        }

        [HttpGet("{code:minlength(1)}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            string userId = _sharedIdentityService.UserId;
            var discountResponseModel = await _discountService.GetByCodeAndUserIdAsync(code, userId);

            return CreateActionResultInstance(discountResponseModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(DiscountRequestModel discountRequestModel)
        {
            var result = await _discountService.AddAsync(discountRequestModel);
            return CreateActionResultInstance(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(DiscountRequestModel discountRequestModel)
        {
            var result = await _discountService.UpdateAsync(discountRequestModel);
            return CreateActionResultInstance(result);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountService.DeleteAsync(id);
            return CreateActionResultInstance(result);
        }
    }
}
