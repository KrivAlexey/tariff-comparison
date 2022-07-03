using System.ComponentModel.DataAnnotations;

namespace TariffComparison.Models;

/// <summary>
///  Calculated annual cost for the tariff 
/// </summary>
public sealed record TariffAnnualCost
{
    /// <summary>
    /// Tariff name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? TariffName { get; init; }
    
    /// <summary>
    /// Calculated annual cost 
    /// </summary>
    [Required]
    public double? AnnualCost { get; init; }
}