using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Admin.Controllers;

public class SettingsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
