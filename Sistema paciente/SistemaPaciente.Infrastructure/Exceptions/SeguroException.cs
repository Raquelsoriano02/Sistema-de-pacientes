namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class SeguroException : Exception
    {
        public SeguroException()
        {
        }

        public SeguroException(string? message) : base(message)
        {
        }

        public SeguroException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
