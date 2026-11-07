namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class EspecialidadException : Exception
    {
        public EspecialidadException()
        {
        }

        public EspecialidadException(string? message) : base(message)
        {
        }

        public EspecialidadException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
