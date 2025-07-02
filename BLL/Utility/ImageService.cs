using Microsoft.AspNetCore.Http;

namespace BLL.Utility;

public class ImageService
{
    public string SaveImageService(IFormFile profileImage)
    {
        if (profileImage != null && profileImage.Length > 0)
        {
            var fileName = Path.GetFileName(profileImage.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                profileImage.CopyTo(fileStream);
            }
            return fileName;
        }
        return null;
    }
}
