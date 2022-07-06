using TariffComparison.Models;

namespace TariffComparison;

/// <summary>
/// TariffStorage
/// </summary>
public interface ITariffStorage
{
    /// <summary>
    /// Get All available tariffs
    /// </summary>
    IReadOnlyList<Tariff> GetAllTariffs();
}

internal class TariffStorage : ITariffStorage
{
    private readonly IReadOnlyList<Tariff> _availableTariffs;

    public TariffStorage()
    {
        _availableTariffs = InitTariffs();
    }

    public IReadOnlyList<Tariff> GetAllTariffs()
    {
        return _availableTariffs;
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