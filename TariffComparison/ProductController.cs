using System.Net.Mime;
using CalculationModelCalculator;
using Microsoft.AspNetCore.Mvc;

namespace TariffComparison;

[ApiController]
[Route("products")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ProductController : ControllerBase
{
    private readonly Calculator _calculator;

    public ProductController(Calculator calculator)
    {
        _calculator = calculator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            new Product("Product A", "base costs per month 5 € + consumption costs 22 cent/kWh"),
            new Product("Product B",
                "800 € for up to 4000 kWh/year and above 4000 kWh/year additionally 30cent/kWh."),
        });
    }

    [HttpPost]
    public IActionResult CompareTariffs([FromBody] double consumption)
    {
        var products = new[]
        {
            new Product("Product A", "60.0 + 0.22 * X"),
            new Product("Product B", "if X <= 4000.0 then 800.0 else 800.0 + (X - 4000.0) * 0.3")
        };

        var comparisonResults = new List<ComparisonResponseItem>();
        foreach (var product in products)
        {
            if (!_calculator.TryEvaluateCalculationModel(product.CalculationModel, consumption, out var result))
            {
                //todo logs 
                continue;
            }
            comparisonResults.Add(new ComparisonResponseItem(product.Name, result!.Value));
        }
        
        return Ok(comparisonResults.OrderBy(c => c.AnnualCost));
    }
}