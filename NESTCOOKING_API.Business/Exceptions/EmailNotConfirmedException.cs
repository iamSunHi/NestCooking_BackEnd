using System;

namespace NESTCOOKING_API.Business.Exceptions
{
    public class EmailNotConfirmedException : Exception
    {
        public EmailNotConfirmedException(string message) : base(message) { }
    }
}