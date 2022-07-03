using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace TariffComparison.Models;

/// <summary>
/// Request for tariff comparison
/// </summary>
[PublicAPI]
public sealed record TariffComparisonRequest
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="annualConsumption">Consumption kWh/year</param>
    public TariffComparisonRequest(double annualConsumption)
    {
        AnnualConsumption = annualConsumption;
    }

    private const double WholeWorldConsumption = 25000000000000;
    /// <summary>
    /// Annual consumption kWh/year
    /// </summary>
    [Required]
    [Range(1, WholeWorldConsumption)]
    public double AnnualConsumption { get; }
}