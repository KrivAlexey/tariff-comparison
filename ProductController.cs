using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace TariffComparison;

[ApiController]
[Route("products")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new []
        {
            new Product("Product A", "base costs per month 5 € + consumption costs 22 cent/kWh"),
            new Product("Product B", 
                "800 € for up to 4000 kWh/year and above 4000 kWh/year additionally 30cent/kWh."),
        });
    }
}