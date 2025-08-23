namespace Hotel_Booking_App.ExceptionHanlder;
// Exceptions/NotFoundException.cs
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// Exceptions/ValidationException.cs
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

// Exceptions/ConflictException.cs
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
