using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs.Posts;

namespace WebApp.ViewComponents;

public class PostItemViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(PostResponse response)
    {
        return View(response);
    }
}
