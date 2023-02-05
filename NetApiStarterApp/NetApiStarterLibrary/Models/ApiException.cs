using System;
namespace NetApiStarterApp.NetApiStarterLibrary.Models
{
	public class ApiServiceException : Exception
	{
        public ApiServiceException()
        {
        }

        public ApiServiceException(string message) : base(message)
        {
        }

    }

    public class ApiNotFoundException : Exception
    {
        public ApiNotFoundException()
        {
        }

        public ApiNotFoundException(string message) : base(message)
        {
        }

    }

    public class ApiInvalidDataException : Exception
    {
        public ApiInvalidDataException()
        {
        }

        public ApiInvalidDataException(string message) : base(message)
        {
        }

    }
}

