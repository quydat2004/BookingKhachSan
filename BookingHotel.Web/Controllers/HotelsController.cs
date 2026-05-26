using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Web.Controllers;

public class HotelsController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Details() => View();
}
