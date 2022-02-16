namespace CourseSales.Services.Discount.Services
{
    public sealed class DiscountService : IDiscountService
    {
        private readonly IDbConnection _dbConnection;

        public DiscountService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Response<NoContentResponse>> DeleteAsync(int id)
        {
            bool succeeded = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id }) > 0;
            if (succeeded)
                return Response<NoContentResponse>.Success(HttpStatusCode.NoContent);

            return Response<NoContentResponse>.Fail("Discount not found.", HttpStatusCode.NoContent);
        }

        public async Task<Response<List<DiscountResponseModel>>> GetAllAsync()
        {
            var discounts = await _dbConnection.QueryAsync<Entities.Discount>("Select * from discount");
            if (!discounts.Any())
                return Response<List<DiscountResponseModel>>.Fail("Discount not found.", HttpStatusCode.NotFound);

            var discountsResponseModel = discounts.ToList().Adapt<List<DiscountResponseModel>>();
            return Response<List<DiscountResponseModel>>.Success(discountsResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<DiscountResponseModel>> GetByCodeAndUserIdAsync(string code, string userId)
        {
            var discount = await _dbConnection.QueryFirstOrDefaultAsync<Entities.Discount>("select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = code });
            if (discount is null)
            {
                return Response<DiscountResponseModel>.Fail("Discount not found.", HttpStatusCode.NotFound);
            }

            var discountResponseModel = discount.Adapt<DiscountResponseModel>();
            return Response<DiscountResponseModel>.Success(discountResponseModel, HttpStatusCode.NotFound);
        }

        public async Task<Response<DiscountResponseModel>> GetByIdAsync(int id)
        {
            var discount = await _dbConnection.QuerySingleOrDefaultAsync<Entities.Discount>("select * from discount where id=@Id", new { Id = id });
            if (discount is null)
            {
                return Response<DiscountResponseModel>.Fail("Discount not found.", HttpStatusCode.NotFound);
            }

            var discountResponseModel = discount.Adapt<DiscountResponseModel>();
            return Response<DiscountResponseModel>.Success(discountResponseModel, HttpStatusCode.NotFound);
        }

        public async Task<Response<NoContentResponse>> AddAsync(DiscountRequestModel discountRequestModel)
        {
            var discount = discountRequestModel.Adapt<Entities.Discount>();
            bool succeeded = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount) > 0;
            if (succeeded)
                return Response<NoContentResponse>.Success(HttpStatusCode.Created);

            return Response<NoContentResponse>.Fail("An error occurred while adding.", HttpStatusCode.InternalServerError);
        }

        public async Task<Response<NoContentResponse>> UpdateAsync(DiscountRequestModel discountRequestModel)
        {
            var discount = discountRequestModel.Adapt<Entities.Discount>();
            var succeeded = await _dbConnection.ExecuteAsync("update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id", new { discount.Id, discount.UserId, discount.Code, discount.Rate }) > 0;
            if (succeeded)
                return Response<NoContentResponse>.Success(HttpStatusCode.NoContent);

            return Response<NoContentResponse>.Fail("An error occurred while updating.", HttpStatusCode.InternalServerError);
        }
    }
}
