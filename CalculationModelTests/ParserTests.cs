using CalculationModelParser;

namespace CalculationModelTests;

public class Tests
{
    [Test]
    public void TokenizerAddSubtractTest()
    {
        var testString = "10 + 20 - 30.123";
        var t = new Tokenizer(new StringReader(testString));

        // "10"
        Assert.That(t.Token, Is.EqualTo(Token.Number));
        Assert.That(t.Number, Is.EqualTo(10));
        t.NextToken();

        // "+"
        Assert.That(t.Token, Is.EqualTo(Token.Add));
        t.NextToken();

        // "20"
        Assert.That(t.Token, Is.EqualTo(Token.Number));
        Assert.That(t.Number, Is.EqualTo(20));
        t.NextToken();
        
        // "-"
        Assert.That(t.Token, Is.EqualTo(Token.Subtract));
        t.NextToken();
        
        // "30.123"
        Assert.That(t.Token, Is.EqualTo(Token.Number));
        Assert.That(t.Number, Is.EqualTo(30.123));
        t.NextToken();
        
        Assert.That(t.Token, Is.EqualTo(Token.EOF));
    }

    [Test]
    public void ParserAddSubtractTest()
    {
        var str = "3 + 1-2";
        var tokenizer = new Tokenizer(new StringReader(str));
        var parser = new Parser(tokenizer);

        var node = parser.ParseExpression();

        var result = node.Eval();
        
        Assert.That(result, Is.EqualTo(2));
    }
}