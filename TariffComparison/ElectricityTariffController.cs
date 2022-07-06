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
    private readonly ITariffStorage _tariffStorage;
    private readonly ILogger<ElectricityTariffController> _logger;

    /// <summary>
    /// Controller for retrieving and comparing electricity tariffs
    /// </summary>
    /// <param name="calculator"></param>
    /// <param name="tariffStorage"></param>
    /// <param name="logger"></param>
    public ElectricityTariffController(
        Calculator calculator, 
        ITariffStorage tariffStorage,
        ILogger<ElectricityTariffController> logger)
    {
        _calculator = calculator;
        _tariffStorage = tariffStorage;
        _logger = logger;
    }

    /// <summary>
    /// Get all available tariffs
    /// </summary>
    /// <response code="200">List of all available for comparison tariffs</response>
    [HttpGet]
    [ProducesResponseType(typeof(Tariff[]), 200)]
    public IActionResult GetAll()
    {
        return Ok(_tariffStorage.GetAllTariffs());
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
        foreach (var product in _tariffStorage.GetAllTariffs())
        {
            if (!_calculator.TryEvaluateCalculationModel(
                    product.CalculationModelFormula, 
                    request.AnnualConsumption,
                    out var result) || !result.HasValue)
            {
                _logger.LogWarning("Can't calculate annual cost for the tariff: {@TariffName}", product.Name);
                continue;
            }
            tariffsAnnualCost.Add(new TariffAnnualCost(product.Name, result.Value));
        }
        
        return Ok(tariffsAnnualCost.OrderBy(c => c.AnnualCost));
    }
}