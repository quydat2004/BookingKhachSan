using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Admin.Controllers;

public class PaymentsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
