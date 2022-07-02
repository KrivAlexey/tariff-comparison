using System.Globalization;
using sly.parser;
using sly.parser.generator;

namespace CalculationModelCalculator;

public class Calculator
{
    private readonly Parser<ExpressionToken, double> _parser = null!;
    
    public Calculator()
    {
        var expressionParserDefinition = new ExpressionParser();
        var builder = new ParserBuilder<ExpressionToken, double>();

        var parserResult = builder.BuildParser(expressionParserDefinition,
            ParserType.LL_RECURSIVE_DESCENT,
            "expression");
        if (parserResult.IsOk) {
            // everythin'fine : we have a configured parser
            _parser = parserResult.Result;
        }
        else {
            // something's wrong
            foreach(var error in parserResult.Errors) {
                Console.WriteLine($"{error.Code} : {error.Message}");
            }    
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