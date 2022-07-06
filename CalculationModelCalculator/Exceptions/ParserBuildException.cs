using System.Runtime.Serialization;
using System.Text;
using sly.buildresult;

namespace CalculationModelCalculator.Exceptions;

public class ParserBuildException : Exception
{
    public ParserBuildException()
    {
    }

    protected ParserBuildException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ParserBuildException(string? message) : base(message)
    {
    }

    public ParserBuildException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public ParserBuildException(IReadOnlyList<InitializationError> errorsList) : this(ErrorsToString(errorsList))
    {
    }
    
    private static string ErrorsToString(IReadOnlyList<InitializationError> errorsList)
    {
        var sb = new StringBuilder();
        sb.Append("Unable to build parser because of following issues");
        foreach (var error in errorsList)
        {
            sb.AppendLine($"Level:{error.Level} Code:{error.Code} Message: {error.Message}");
        }

        return sb.ToString();
    }
}