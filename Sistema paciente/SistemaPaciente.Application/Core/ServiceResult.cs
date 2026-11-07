namespace SistemaPaciente.Application.Core
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public bool NotFound { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T> { Success = true, Data = data };
        }

        public static ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T> { Message = message };
        }

        public static ServiceResult<T> NotFoundResult(string message)
        {
            return new ServiceResult<T> { NotFound = true, Message = message };
        }
    }
}
