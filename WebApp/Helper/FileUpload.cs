using System.Text.RegularExpressions;

namespace WebApp.Helper;

public partial class FileUpload
{
    public static async Task<string?> UploadHelper(IFormFile uploadedFile, IWebHostEnvironment environment)
    {
        const string storageLoc = "fileUpload";
        string filename = Guid.NewGuid() + uploadedFile.FileName;
        
        string path = Path.Combine(environment.WebRootPath, storageLoc);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        
        if (uploadedFile.Length == 0) return null;
        if (!MyRegex().IsMatch(uploadedFile.ContentType)) return null;

        await using FileStream stream = new(Path.Combine(path, filename), FileMode.Create);
        await uploadedFile.CopyToAsync(stream);
        return Path.Combine("/", storageLoc, filename);
    }

    [GeneratedRegex("image")]
    private static partial Regex MyRegex();
}