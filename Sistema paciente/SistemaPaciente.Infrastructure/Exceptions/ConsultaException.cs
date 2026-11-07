namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class ConsultaException : Exception
    {
        public ConsultaException()
        {
        }

        public ConsultaException(string? message) : base(message)
        {
        }

        public ConsultaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
