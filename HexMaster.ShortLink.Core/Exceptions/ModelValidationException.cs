using System;
using System.Collections.Generic;
using HexMaster.ShortLink.Core.Enums;
using Microsoft.Azure.Documents;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public class ModelValidationException : Exception
    {
        private readonly IReadOnlyList<ErrorCode> _errors;

        public ModelValidationException(
            List<ErrorCode> errors,
            string message = "Found a validation error",
            Exception innerException = null)
        :base(message, innerException)
        {
            _errors = errors;
        }
    }
}