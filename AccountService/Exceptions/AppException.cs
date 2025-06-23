namespace AccountService.Exceptions;

public abstract class AppException : Exception
{
    public abstract int StatusCode { get; }
    public abstract string ErrorCode { get; }
    protected AppException(string message) : base(message)
    {
    }
    public sealed class NotFoundException : AppException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
        public override string ErrorCode => "NotFound";
        public NotFoundException(string message) : base(message)
        {
        }

    }
    public sealed class ValidationException : AppException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;
        public override string ErrorCode => "VALIDATION_FAILED";
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(Dictionary<string, string[]> errors)
            : base("Validation errors occurred")
            => Errors = errors;
    }
    public sealed class EmailAlreadyExistsException : AppException
    {
        public override int StatusCode => StatusCodes.Status409Conflict;
        public override string ErrorCode => "EMAIL_ALREADY_EXISTS";
        public EmailAlreadyExistsException(string message) : base(message)
        {
        }
    }
}