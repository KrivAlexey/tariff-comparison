using System.Globalization;
using CalculationModelCalculator.Exceptions;
using Microsoft.Extensions.Logging;
using sly.parser;
using sly.parser.generator;

namespace CalculationModelCalculator;

public class Calculator
{
    private readonly Parser<ExpressionToken, double> _parser;
    private readonly ILogger<Calculator> _logger;

    public Calculator(ILogger<Calculator> logger)
    {
        _logger = logger;
        var expressionParserDefinition = new ExpressionParser();
        var builder = new ParserBuilder<ExpressionToken, double>();

        var parserResult = builder.BuildParser(expressionParserDefinition,
            ParserType.LL_RECURSIVE_DESCENT,
            "expression");
        if (parserResult.IsOk)
        {
            // everythin'fine : we have a configured parser
            _parser = parserResult.Result;
        }
        else
        {
            throw new ParserBuildException(parserResult.Errors);
        }
    }

    public bool TryEvaluateCalculationModel(string calculationFormula, double? parameter, out double? result)
    {
        result = null;
        var parseResult = _parser.Parse(ReplaceParameter(calculationFormula, parameter));
        if (parseResult.IsOk)
        {
            result = parseResult.Result;
        }
        else
        {
            _logger.LogWarning("Can't calculate formula: {@CalculationFormula} with parameter {@Parameter}. Errors: {@Errors}", 
                calculationFormula, parameter, parseResult.Errors);
        }

        return parseResult.IsOk;
    }

    private string ReplaceParameter(string calculationFormula, double? parameter)
    {
        if (parameter != null)
        {
            return calculationFormula.Replace("X",
                parameter.Value.ToString("000000.000", CultureInfo.InvariantCulture));
        }

        return calculationFormula;
    }
}