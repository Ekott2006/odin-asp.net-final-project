using Microsoft.AspNetCore.Mvc;
using WebApp.Model.ViewModel;

namespace WebApp.ViewComponents;

public class UploadImageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(UploadImageViewModel viewModel) {
        return View(viewModel);
    }
}
