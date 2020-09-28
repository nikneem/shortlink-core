using System;
using System.Collections.Generic;
using HexMaster.ShortLink.Core.Enums;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public class ModelValidationException : Exception
    {
        public IReadOnlyList<ErrorCode> Errors { get; }

        public ModelValidationException(
            List<ErrorCode> errors,
            string message = "Found a validation error",
            Exception innerException = null)
        :base(message, innerException)
        {
            Errors = errors;
        }
    }
}