using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CalculationModelCalculator;
using Microsoft.AspNetCore.Mvc;
using TariffComparison.Models;

namespace TariffComparison;

/// <summary>
/// Controller for retrieving and comparing electricity tariffs
/// </summary>
[ApiController]
[Route("tariffs")]
[ApiVersion("1")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ElectricityTariffController : ControllerBase
{
    private readonly Calculator _calculator;
    private readonly IReadOnlyList<Tariff> _availableTariffs;

    /// <summary>
    /// Controller for retrieving and comparing electricity tariffs
    /// </summary>
    /// <param name="calculator"></param>
    public ElectricityTariffController(Calculator calculator)
    {
        _calculator = calculator;
        _availableTariffs = InitTariffs();
    }

    /// <summary>
    /// Get all available tariffs
    /// </summary>
    /// <response code="200">List of all available for comparison tariffs</response>
    [HttpGet]
    [ProducesResponseType(typeof(Tariff[]), 200)]
    public IActionResult GetAll()
    {
        return Ok(_availableTariffs);
    }

    /// <summary>
    /// Compare tariffs
    /// </summary>
    /// <response code="200">List of annual costs of all tariffs for requested consumption</response>
    /// <response code="400">Validation error</response>
    [HttpPost]
    [Route("compare")]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(TariffAnnualCost[]), 200)]
    public IActionResult CompareTariffs([Required][FromBody] TariffComparisonRequest request)
    {
        var tariffsAnnualCost = new List<TariffAnnualCost>();
        foreach (var product in _availableTariffs)
        {
            if (!_calculator.TryEvaluateCalculationModel(
                    product.CalculationModelFormula, 
                    request.AnnualConsumption,
                    out var result) || !result.HasValue)
            {
                //todo logs 
                continue;
            }
            tariffsAnnualCost.Add(new TariffAnnualCost(product.Name, result.Value));
        }
        
        return Ok(tariffsAnnualCost.OrderBy(c => c.AnnualCost));
    }
    
    private static IReadOnlyList<Tariff> InitTariffs()
    {
        return new[]
        {
            new Tariff(
                "basic electricity tariff",
                "base costs per month 5 € + consumption costs 22 cent/kWh",
                "60.0 + 0.22 * X",
                @"
• Consumption = 3500 kWh/year => Annual costs = 830 €/year (5€ * 12 months = 60 € base
costs + 3500 kWh/year * 22 cent/kWh = 770 € consumption costs)
• Consumption = 4500 kWh/year => Annual costs = 1050 €/year (5€ * 12 months = 60 € base
costs + 4500 kWh/year * 22 cent/kWh = 990 € consumption costs)
• Consumption = 6000 kWh/year => Annual costs = 1380 €/year (5€ * 12 months = 60 € base
costs + 6000 kWh/year * 22 cent/kWh = 1320 € consumption costs)"
                ),
            new Tariff(
                "Packaged tariff",
                "800 € for up to 4000 kWh/year and above 4000 kWh/year additionally 30cent/kWh.",
                "if X <= 4000.0 then 800.0 else 800.0 + (X - 4000.0) * 0.3",
                @"
• Consumption = 3500 kWh/year => Annual costs = 800 €/year
• Consumption = 4500 kWh/year => Annual costs = 950 €/year (800€ + 500 kWh * 30 cent/kWh
= 150 € additional consumption costs)
• Consumption = 6000 kWh/year => Annual costs = 1400 €/year (800€ + 2000 kWh * 30
cent/kWh = 600 € additional consumption costs)")
        };
    }
}