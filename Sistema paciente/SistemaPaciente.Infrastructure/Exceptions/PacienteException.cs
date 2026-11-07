namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class PacienteException : Exception
    {
        public PacienteException()
        {
        }

        public PacienteException(string? message) : base(message)
        {
        }

        public PacienteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
