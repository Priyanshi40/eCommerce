using Microsoft.AspNetCore.Http;

namespace BLL.Utility;

public class ImageService
{
    public string SaveImageService(IFormFile profileImage,string subFolder = "images")
    {
        if (profileImage != null && profileImage.Length > 0)
        {
            string fileName = Path.GetFileName(profileImage.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",subFolder, fileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                profileImage.CopyTo(fileStream);
            }
            return fileName;
        }
        return null;
    }
}
