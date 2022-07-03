using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace TariffComparison.Models;

/// <summary>
/// Electricity tariff
/// </summary>
[PublicAPI]
public sealed record Tariff
{
    /// <summary>
    /// Electricity tariff
    /// </summary>
    /// <param name="name"></param>
    /// <param name="calculationModel"></param>
    /// <param name="calculationModelFormula"></param>
    /// <param name="calculationExamples"></param>
    public Tariff(string name, string calculationModel, string calculationModelFormula, string? calculationExamples = null)
    {
        Name = name;
        CalculationModel = calculationModel;
        CalculationModelFormula = calculationModelFormula;
        CalculationExamples = calculationExamples;
    }

    /// <summary>
    /// Tariff name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; }
    
    /// <summary>
    /// Human readable calculation model
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string CalculationModel { get; init; }
    
    /// <summary>
    /// Calculation model formula
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string CalculationModelFormula { get; init; }
    
    /// <summary>
    /// CalculationExamples
    /// </summary>
    public string? CalculationExamples { get; init; }
}