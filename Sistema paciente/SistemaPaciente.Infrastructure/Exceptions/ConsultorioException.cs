namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class ConsultorioException : Exception
    {
        public ConsultorioException()
        {
        }

        public ConsultorioException(string? message) : base(message)
        {
        }

        public ConsultorioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
