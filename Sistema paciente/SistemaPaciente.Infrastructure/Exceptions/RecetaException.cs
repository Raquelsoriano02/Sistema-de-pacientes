namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class RecetaException : Exception
    {
        public RecetaException()
        {
        }

        public RecetaException(string? message) : base(message)
        {
        }

        public RecetaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
