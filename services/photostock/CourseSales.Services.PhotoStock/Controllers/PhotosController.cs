namespace CourseSales.Services.PhotoStock.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(typeof(Response<PhotoResponseModel>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Response<PhotoResponseModel>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo is null || 0 >= photo?.Length)
                return CreateActionResultInstance(Response<PhotoResponseModel>.Fail("Photo is empty!", HttpStatusCode.BadRequest));

            string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos/", photo.FileName);
            
            using FileStream fileStream = new(photoPath, FileMode.Create);
            await photo.CopyToAsync(fileStream, cancellationToken);

            PhotoResponseModel photoResponseModel = new($"photos/{photo.FileName}");

            return CreateActionResultInstance(Response<PhotoResponseModel>.Success(photoResponseModel, HttpStatusCode.Created));
        }

        [HttpDelete("{photoUrl:minlength(5)}")]
        [ProducesResponseType(typeof(Response<PhotoResponseModel>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Response<PhotoResponseModel>), (int)HttpStatusCode.NoContent)]
        public IActionResult Delete(string photoUrl)
        {
            string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos/", photoUrl);
            if (!System.IO.File.Exists(photoPath))
                return CreateActionResultInstance(Response<NoContentResponse>.Fail("Photo not found.", HttpStatusCode.NotFound));

            System.IO.File.Delete(photoPath);

            return CreateActionResultInstance(Response<NoContentResponse>.Success(HttpStatusCode.NoContent));
        }
    }
}
