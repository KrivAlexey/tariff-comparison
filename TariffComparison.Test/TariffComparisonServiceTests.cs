using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using TariffComparison.Models;

namespace TariffComparison.Test;

public class TariffComparisonServiceTests
{
    [Test]
    public async Task GetAllTariffsTest()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/tariffs");
        Assert.IsTrue(response.IsSuccessStatusCode);
        
        var tariffs = JsonSerializer.Deserialize<IReadOnlyList<Tariff>>(await response.Content.ReadAsStreamAsync());
        Assert.IsNotNull(tariffs);
        Assert.IsTrue(tariffs!.Count == 2);
    }
    
    [Test]
    public async Task When_AnnualConsumption_Zero_Then_CompareTariffs_BadRequest()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var request = new TariffComparisonRequest(0);

        var response = await client.PostAsync("/tariffs/compare", 
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
                ));
        
        Assert.IsFalse(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(await response.Content.ReadAsStreamAsync());
        Assert.IsNotNull(problemDetails);
        Assert.That(problemDetails!.Errors.Keys.Single(), Is.EqualTo("AnnualConsumption"));
    }
    
    [Test]
    public async Task When_AnnualConsumption_Overflows_Then_CompareTariffs_BadRequest()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var request = new TariffComparisonRequest(25000000000001);

        var response = await client.PostAsync("/tariffs/compare", 
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
            ));
        
        Assert.IsFalse(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(await response.Content.ReadAsStreamAsync());
        Assert.IsNotNull(problemDetails);
        Assert.That(problemDetails!.Errors.Keys.Single(), Is.EqualTo("AnnualConsumption"));
    }

    [Test]
    public async Task When_AnnualConsumption_Valid_Then_Ok()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var request = new TariffComparisonRequest(4000);

        var response = await client.PostAsync("/tariffs/compare", 
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
            ));
        
        Assert.IsTrue(response.IsSuccessStatusCode);
    }
}