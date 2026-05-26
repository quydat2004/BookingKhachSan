using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Admin.Controllers;

public class HotelsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
