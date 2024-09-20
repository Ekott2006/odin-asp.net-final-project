using System.ComponentModel.DataAnnotations;

namespace WebApp.Model.ViewModel;

public class CreatePostViewModelRequest
{
    [Required, Length(2, 200)]
    public string Content { get; set; }
    [Required]
    public string ReturnUrl { get; set; }

}
