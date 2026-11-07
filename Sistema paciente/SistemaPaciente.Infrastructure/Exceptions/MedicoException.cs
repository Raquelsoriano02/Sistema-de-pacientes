namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class MedicoException : Exception
    {
        public MedicoException()
        {
        }

        public MedicoException(string? message) : base(message)
        {
        }

        public MedicoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
