using System.Runtime.Serialization;

namespace Exceptions;

[Serializable]
public sealed class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string? message): base(message) {}
    public AlreadyExistsException(string? message, Exception inner): base(message, inner) {}
    protected AlreadyExistsException(SerializationInfo info, StreamingContext context): base(info, context) { }
}