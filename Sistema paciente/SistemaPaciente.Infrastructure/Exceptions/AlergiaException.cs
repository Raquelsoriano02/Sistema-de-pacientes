namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class AlergiaException : Exception
    {
        public AlergiaException()
        {
        }

        public AlergiaException(string? message) : base(message)
        {
        }

        public AlergiaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
