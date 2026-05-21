using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace Lazada_Isagunde.Blazor.Services;

public class ImageService
{
    private const string CloudName = "dmulmfocc";
    private const string ApiKey = "855523926928672";
    private const string ApiSecret = "xdLGVJcLvMPJQWYB3gLlBVSKdjU";

    private readonly HttpClient _httpClient;

    public ImageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> UploadImageAsync(IBrowserFile file)
    {
        if (file == null) return string.Empty;

        try
        {
            // 1. Read stream into memory safely (Max 10MB)
            using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            byte[] bytes = ms.ToArray();

            // 2. Convert to Base64 (Data URI)
            // This is the most reliable way for Blazor WASM
            string base64 = Convert.ToBase64String(bytes);
            string dataUri = $"data:{file.ContentType};base64,{base64}";

            // 3. Prepare Parameters
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var folder = "lazada_products";

            // 4. Create Signature
            // Cloudinary requires signed parameters in alphabetical order: folder=...&timestamp=...[secret]
            var signatureString = $"folder={folder}&timestamp={timestamp}{ApiSecret}";
            var signature = ComputeSha1Hash(signatureString);

            // 5. Prepare Form Content
            var formData = new Dictionary<string, string>
            {
                { "file", dataUri },
                { "api_key", ApiKey },
                { "timestamp", timestamp },
                { "signature", signature },
                { "folder", folder }
            };

            var content = new FormUrlEncodedContent(formData);

            // 6. Send Request
            var response = await _httpClient.PostAsync($"https://api.cloudinary.com/v1_1/{CloudName}/image/upload", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Cloudinary Error] Status: {response.StatusCode}");
                Console.WriteLine($"[Cloudinary Error] Body: {errorBody}");
                return string.Empty;
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("secure_url").GetString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Image Upload Exception: {ex.Message}");
            return string.Empty;
        }
    }

    private string ComputeSha1Hash(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = SHA1.HashData(inputBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
