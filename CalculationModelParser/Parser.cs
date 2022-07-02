namespace CalculationModelParser;

public class Parser
{
    // Constructor - just store the tokenizer
    public Parser(Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    Tokenizer _tokenizer;

    // Parse an entire expression and check EOF was reached
    public Node ParseExpression()
    {
        // For the moment, all we understand is add and subtract
        var expr = ParseAddSubtract();

        // Check everything was consumed
        if (_tokenizer.Token != Token.EOF)
            throw new SyntaxException("Unexpected characters at end of expression");

        return expr;
    }

    // Parse an sequence of add/subtract operators
    Node ParseAddSubtract()
    {
        // Parse the left hand side
        var lhs = ParseLeaf();

        while (true)
        {
            // Work out the operator
            Func<double, double, double>? op = _tokenizer.Token
                switch
                {
                    Token.Add => (a, b) => a + b,
                    Token.Subtract => (a, b) => a - b,
                    _ => null
                };

            // Binary operator found?
            if (op == null)
                return lhs;             // no
    
            // Skip the operator
            _tokenizer.NextToken();

            // Parse the right hand side of the expression
            var rhs = ParseLeaf();

            // Create a binary node and use it as the left-hand side from now on
            lhs = new NodeBinary(lhs, rhs, op);
        }
    }

    // Parse a leaf node
    // (For the moment this is just a number)
    Node ParseLeaf()
    {
        // Is it a number?
        if (_tokenizer.Token == Token.Number)
        {
            var node = new NodeNumber(_tokenizer.Number);
            _tokenizer.NextToken();
            return node;
        }

        // Don't Understand
        throw new SyntaxException($"Unexpect token: {_tokenizer.Token}");
    }
}