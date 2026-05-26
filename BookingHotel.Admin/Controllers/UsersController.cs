using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Admin.Controllers;

public class UsersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
