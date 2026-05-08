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

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(filePath),
            Folder = "lazada_products"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult?.SecureUrl?.ToString() ?? string.Empty;
    }
}
