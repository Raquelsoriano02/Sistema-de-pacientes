namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class CitaException : Exception
    {
        public CitaException()
        {
        }

        public CitaException(string? message) : base(message)
        {
        }

        public CitaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
