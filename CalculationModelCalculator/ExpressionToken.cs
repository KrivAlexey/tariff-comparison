using sly.lexer;

namespace CalculationModelCalculator;

internal enum ExpressionToken
{
    [Keyword("if")]
    IF = 1,

    [Keyword("then")]
    THEN = 2,

    [Keyword("else")]
    ELSE = 3,
    
    [Double]
    DOUBLE = 4,

    [Sugar("+")]
    PLUS = 5,

    // the - operator
    [Sugar("-")]
    MINUS = 6,

    [Sugar("*")]
    TIMES = 7,
    
    [Sugar("/")]
    DIVIDE = 8,

    [Sugar("(")]
    LPAREN = 9,
    
    [Sugar(")")]
    RPAREN = 10,
    
    [Sugar( ">")] 
    GREATER = 30,
    
    [Sugar( ">=")]
    GREATER_OR_EQUALS = 31,

    [Sugar( "<")] 
    LESSER = 32,
    
    [Sugar( "<=")] 
    LESSER_OR_EQUALS = 33,

    [Sugar( "==")]
    EQUALS = 34,

    [Sugar( "!=")]
    DIFFERENT = 35,
    
    [Keyword("X")]
    X = 36
}