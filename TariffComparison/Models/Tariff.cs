using System.ComponentModel.DataAnnotations;

namespace TariffComparison.Models;

/// <summary>
/// Electricity tariff
/// </summary>
public sealed record Tariff
{
    /// <summary>
    /// Tariff name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; init; }
    
    /// <summary>
    /// Human readable calculation model
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? CalculationModel { get; init; }
    
    /// <summary>
    /// Calculation model formula
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? CalculationModelFormula { get; init; }
    
    /// <summary>
    /// CalculationExamples
    /// </summary>
    public string? CalculationExamples { get; init; }
}