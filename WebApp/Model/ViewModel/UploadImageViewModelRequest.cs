using System.ComponentModel.DataAnnotations;

namespace WebApp.Model.ViewModel;

public class UploadImageViewModelRequest
{
    public string ReturnUrl { get; set; }
    [Required]
    public IFormFile UploadedFile { get; set; }
    public string? Caption { get; set; }
}
