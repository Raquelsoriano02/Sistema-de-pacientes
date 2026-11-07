namespace SistemaPaciente.Infrastructure.Exceptions
{
    public class MedicamentoException : Exception
    {
        public MedicamentoException()
        {
        }

        public MedicamentoException(string? message) : base(message)
        {
        }

        public MedicamentoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
