namespace CourseSales.Services.PhotoStock.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Save(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo is null || 0 >= photo?.Length)
                return CreateActionResultInstance(Response<PhotoResponseModel>.Fail("Photo is empty!", HttpStatusCode.BadRequest));

            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos/", photo.FileName);
            
            using FileStream fileStream = new(photoPath, FileMode.Create);
            await photo.CopyToAsync(fileStream, cancellationToken);

            PhotoResponseModel photoResponseModel = new($"photos/{photo.FileName}");

            return CreateActionResultInstance(Response<PhotoResponseModel>.Success(photoResponseModel, HttpStatusCode.Created));
        }
    }
}
