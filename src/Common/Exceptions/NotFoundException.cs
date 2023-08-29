using System.Runtime.Serialization;

namespace Exceptions;

[Serializable]
public sealed class NotFoundException : Exception
{
    public NotFoundException(string? message): base(message) {}
    public NotFoundException(string? message, Exception inner): base(message, inner) {}
    protected NotFoundException(SerializationInfo info, StreamingContext context): base(info, context) { }
}