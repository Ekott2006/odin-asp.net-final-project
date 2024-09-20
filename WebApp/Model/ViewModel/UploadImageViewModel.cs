namespace WebApp.Model.ViewModel;

public class UploadImageViewModel(string returnUrl, bool showText, string aspPage = "./Index")
{
    public string ReturnUrl { get; set; } = returnUrl;
    public bool ShowText { get; set; } = showText;
    public string AspPage { get; set; } = aspPage;

    public UploadImageViewModelRequest UploadImageRequest { get; set; }
}
