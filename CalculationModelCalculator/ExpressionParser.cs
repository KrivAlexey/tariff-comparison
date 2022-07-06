using JetBrains.Annotations;
using sly.lexer;
using sly.parser.generator;

namespace CalculationModelCalculator;

[PublicAPI]
internal class ExpressionParser
{
    [Production("primary: DOUBLE")]
    public double Primary(Token<ExpressionToken> doubleToken)
    {
        return doubleToken.DoubleValue;
    }

    [Production("primary: LPAREN [d] expression RPAREN [d]")]
    public double Group(double groupValue)
    {
        return groupValue;
    }

    [Production("expression: term PLUS expression")]
    [Production("expression: term MINUS expression")]
    public double Expression(double left, Token<ExpressionToken> operatorToken, double right)
    {
        var result = operatorToken.TokenID switch
        {
            ExpressionToken.PLUS => left + right,
            ExpressionToken.MINUS => left - right,
            ExpressionToken.TIMES => left * right,
            ExpressionToken.DIVIDE => left / right,
            _ => 0D
        };

        return result;
    }
    
    [Production("term : factor TIMES term")]
    [Production("term : factor DIVIDE term")]
    public double Term(double left, Token<ExpressionToken> operatorToken, double right)
    {
        var result = operatorToken.TokenID switch
        {
            ExpressionToken.TIMES => left * right,
            ExpressionToken.DIVIDE => left / right,
            _ => 0
        };

        return result;
    }
    
    [Production("expression: term")]
    public double Expression_Term(double termValue)
    {
        return termValue;
    }
    
    [Production("term: factor")]
    public double Term_Factor(double factorValue)
    {
        return factorValue;
    }
    
    [Production("factor: primary")]
    public double PrimaryFactor(double primValue)
    {
        return primValue;
    }

    [Production("factor: MINUS factor")]
    public double Factor(Token<ExpressionToken> discardedMinus, double factorValue)
    {
        return - factorValue;
    }

    [Production("expression: IF condition THEN expression ELSE expression")]
    public double Statement(
        Token<ExpressionToken> ifToken,
        double condition,
        Token<ExpressionToken> thenToken,
        double thenExpression,
        Token<ExpressionToken> elseToken,
        double elseExpression)
    {
        if (condition > 0)
        {
            return thenExpression;
        }
        else
        {
            return elseExpression;
        }
    }

    [Production("condition: term GREATER term")]
    [Production("condition: term GREATER_OR_EQUALS term")]
    [Production("condition: term LESSER term")]
    [Production("condition: term LESSER_OR_EQUALS term")]
    [Production("condition: term EQUALS term")]
    [Production("condition: term DIFFERENT term")]
    public double Condition(double left, Token<ExpressionToken> operatorToken,
        double right)
    {
        var result = operatorToken.TokenID switch
        {
            ExpressionToken.LESSER => left < right,
            ExpressionToken.LESSER_OR_EQUALS => left <= right,
            ExpressionToken.GREATER => left > right,
            ExpressionToken.GREATER_OR_EQUALS => left >= right,
            ExpressionToken.EQUALS => Math.Abs(left - right) < double.Epsilon,
            ExpressionToken.DIFFERENT => Math.Abs(left - right) > double.Epsilon,
            _ => false 
        };
        return result ? 1 : 0;
    }
}