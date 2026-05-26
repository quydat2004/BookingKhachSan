using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Web.Controllers;

public class PaymentsController : Controller
{
    public IActionResult Process() => View();

    public IActionResult Success() => View();

    public IActionResult Failed() => View();
}
