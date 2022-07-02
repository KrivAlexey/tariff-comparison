using System.Runtime.Serialization;

namespace CalculationModelParser;

public class SyntaxException : Exception
{
    public SyntaxException()
    {
    }

    protected SyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public SyntaxException(string? message) : base(message)
    {
    }

    public SyntaxException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}