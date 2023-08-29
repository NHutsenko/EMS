using System.Runtime.Serialization;

namespace Exceptions;

[Serializable]
public sealed class BadRequestException: Exception
{
    public BadRequestException(string? message): base(message) {}
    public BadRequestException(string? message, Exception inner): base(message, inner) {}
    protected BadRequestException(SerializationInfo info, StreamingContext context): base(info, context) { }
}