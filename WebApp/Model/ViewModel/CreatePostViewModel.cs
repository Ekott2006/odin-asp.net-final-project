namespace WebApp.Model.ViewModel;

public class CreatePostViewModel(string returnUrl, bool showText = true)
{
    public string ReturnUrl { get; set; } = returnUrl;
    public UploadImageViewModel ImageViewModel { get; set; } = new(returnUrl, showText);

    public CreatePostViewModelRequest CreatePostRequest { get; set; }
}
