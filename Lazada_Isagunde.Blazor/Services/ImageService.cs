using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Components.Forms;

namespace Lazada_Isagunde.Blazor.Services;

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

    public async Task<string> UploadImageAsync(IBrowserFile file)
    {
        if (file == null) return string.Empty;

        try
        {
            // Max size 5MB
            using var stream = file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.Name, stream),
                Folder = "lazada_products"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult?.SecureUrl?.ToString() ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}
