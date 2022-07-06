using Microsoft.Extensions.Logging.Abstractions;

namespace CalculationModelCalculator.Test;

public class Tests
{
    private Calculator _calculator;

    [OneTimeSetUp]
    public void SetUp()
    {
        _calculator = new Calculator(NullLogger<Calculator>.Instance);
    }
    
    [Test]
    public void Paranthesis_Pus_Minus()
    {
        var result = _calculator.TryEvaluateCalculationModel("(3.0-1.0) + 3.0", null, out var resultValue);
        Assert.IsTrue(result);
        Assert.That(resultValue, Is.EqualTo(5));
    }
    
    [Test]
    public void Paranthesis_Times_Minus()
    {
        var result = _calculator.TryEvaluateCalculationModel("(3.0-1.0) * 3.0", null, out var resultValue);
        Assert.IsTrue(result);
        Assert.That(resultValue, Is.EqualTo(6));
    }
    
    [Test]
    public void Paranthesis_Divide_Minus()
    {
        var result = _calculator.TryEvaluateCalculationModel("(13.0-1.0) / 3.0", null, out var resultValue);
        Assert.IsTrue(result);
        Assert.That(resultValue, Is.EqualTo(4));
    }
    
    [Test]
    public void Plus_Times_Priority()
    {
        var result = _calculator.TryEvaluateCalculationModel("1.0 + 2.0 * 3.0", null, out var resultValue);
        Assert.IsTrue(result);
        Assert.That(resultValue, Is.EqualTo(7));
    }

    [Test]
    public void Greater_Then()
    {
        var result = _calculator.TryEvaluateCalculationModel("if 4.0 > 2.0 then 5.0 else 10.0", null, out var resultValue);
        Assert.IsTrue(result);
        Assert.That(resultValue, Is.EqualTo(5));
    }

    [Test]
    public void Lesser_Else()
    {
        var result = _calculator.TryEvaluateCalculationModel("if 4.0 < 2.0 then 5.0 else 10.0", null, out var resultValue);
        Assert.IsTrue(result);
        Assert.That(resultValue, Is.EqualTo(10));
    }
    
    [Test]
    public void LesserOrEquals_Then()
    {
        var result = _calculator.TryEvaluateCalculationModel("if 2.0 <= 2.0 then 5.0 else 10.0", null, out var resultValue);
        Assert.IsTrue(result);
        Assert.That(resultValue, Is.EqualTo(5));
    }

    [TestCase(3500, ExpectedResult = 830)]
    [TestCase(4500, ExpectedResult = 1050)]
    [TestCase(6000, ExpectedResult = 1380)]
    public double ProductA_ComparisonModel(double parameter)
    {
        var result = _calculator.TryEvaluateCalculationModel("60.0 + 0.22 * X", parameter, out var resultValue);
        Assert.IsTrue(result);
        return resultValue!.Value;
    }

    [TestCase(3500, ExpectedResult = 800)]
    [TestCase(4500, ExpectedResult = 950)]
    [TestCase(6000, ExpectedResult = 1400)]
    public double ProductB_ComparisonModel(double parameter)
    {
        var result = _calculator.TryEvaluateCalculationModel(
            "if X <= 4000.0 then 800.0 else 800.0 + (X - 4000.0) * 0.3", parameter, out var resultValue);
        Assert.IsTrue(result);
        return resultValue!.Value;
    }
}