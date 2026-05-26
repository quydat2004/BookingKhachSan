using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Web.Controllers;

public class ManagerController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Bookings() => View();

    public IActionResult Payments() => View();

    public IActionResult Reviews() => View();
}
