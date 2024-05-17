namespace Tools;

public class CustomException
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException(): base(){}
        public InvalidDataException(string statuscode,string message) : base(message){}
        public InvalidDataException(string message, Exception innerException):base(message, innerException){}
        
    }
}