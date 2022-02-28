namespace CourseSales.Web.Services.PhotoStocks
{
    public sealed class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhotoAsync(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos/delete/{photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhotoAsync(IFormFile photo)
        {
            if (photo is null || 0 >= photo.Length)
                return null;

            var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";

            using MemoryStream memoryStream = new();
            await photo.CopyToAsync(memoryStream);

            MultipartFormDataContent multipartFormDataContent = new();
            multipartFormDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "photo", randomFileName);

            var httpResponseMessage = await _httpClient.PostAsync("photos/add", multipartFormDataContent);
            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            var uploadPhotoResponse = await httpResponseMessage.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();

            return uploadPhotoResponse.Data;
        }
    }
}
