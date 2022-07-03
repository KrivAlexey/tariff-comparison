using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace TariffComparison.Models;

/// <summary>
///  Calculated annual cost for the tariff 
/// </summary>
[PublicAPI]
public sealed record TariffAnnualCost
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tariffName">Tariff name</param>
    /// <param name="annualCost">Calculated annual cost </param>
    public TariffAnnualCost(string tariffName, double annualCost)
    {
        TariffName = tariffName;
        AnnualCost = annualCost;
    }

    /// <summary>
    /// Tariff name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string TariffName { get; }
    
    /// <summary>
    /// Calculated annual cost 
    /// </summary>
    [Required]
    public double AnnualCost { get; }
}