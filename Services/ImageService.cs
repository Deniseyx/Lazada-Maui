using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Lazada_Isagunde.Services;

public class ImageService
{
    private const string CloudName = "dmulmfocc";
    private const string ApiKey = "855523926928672";
    private const string ApiSecret = "xdLGVJcLvMPJQWYB3gLlBVSKdjU";

    private readonly Cloudinary _cloudinary;

    public ImageService()
    {
        Account account = new Account(CloudName, ApiKey, ApiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadImageAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return string.Empty;

        try
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath),
                Folder = "lazada_products"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            
            if (uploadResult.Error != null)
            {
                Console.WriteLine($"Cloudinary MAUI Error: {uploadResult.Error.Message}");
                return string.Empty;
            }

            return uploadResult?.SecureUrl?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Image Upload MAUI Exception: {ex.Message}");
            return string.Empty;
        }
    }
}
