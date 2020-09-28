using System;
using System.Collections.Generic;
using HexMaster.ShortLink.Core.Enums;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public class ModelValidationException : ShortLinkException
    {
        public ModelValidationException(
            List<ErrorCode> errors,
            string message = "Found a validation error",
            Exception innerException = null)
            : base(errors, message, innerException)
        {
        }
    }
}