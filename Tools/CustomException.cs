namespace Tools;

public class CustomException
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException() : base(){}
        public InvalidDataException(string message) : base(message) { }
        public InvalidDataException(string statuscode,string message) : base(message){}
        public InvalidDataException(string message, Exception innerException):base(message, innerException){}
        
    }

    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException():base(){}
        public InternalServerErrorException(string message) : base(message){}
        public InternalServerErrorException(string message, Exception innerException):base(message, innerException){}
        
    }
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() : base() { }
        public DataNotFoundException(string message) : base(message) { }
        public DataNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string message) : base(message)
        {
        }
    }
}