using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Web.Controllers;

public class BookingsController : Controller
{
    public IActionResult Create() => View();

    public IActionResult My() => View();

    public IActionResult Details() => View();
}
