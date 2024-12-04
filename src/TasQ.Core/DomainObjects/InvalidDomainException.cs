namespace TasQ.Core.DomainObjects;

public class InvalidDomainException : Exception
{
    public InvalidDomainException()
    {
    }

    public InvalidDomainException(string message) : base(message)
    {
    }

    public InvalidDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}