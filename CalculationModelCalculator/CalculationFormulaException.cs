using System.Runtime.Serialization;

namespace CalculationModelCalculator;

public class CalculationFormulaException : Exception
{
    public CalculationFormulaException()
    {
    }

    protected CalculationFormulaException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CalculationFormulaException(string? message) : base(message)
    {
    }

    public CalculationFormulaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}