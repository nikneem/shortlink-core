using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using HexMaster.ShortLink.Core.Enums;
using HexMaster.ShortLink.Core.Exceptions;
using HexMaster.ShortLink.Core.ShortLinks.Models;

namespace HexMaster.ShortLink.Core.Validators
{
    public class ShortLinkUpdateValidator: AbstractValidator<ShortLinkUpdateDto>
    {
        public ShortLinkUpdateValidator()
        {
            RuleFor(x => x.EndpointUrl)
                .NotNull()
                .NotEmpty()
                .Matches(Constants.UrlRegularExpression)
                .WithErrorCode(ErrorCode.InvalidUrl.Code)
                .WithMessage("The endpoint URL is not a valid URL");
            RuleFor(x => x.ShortCode)
                .NotNull()
                .NotEmpty()
                .Matches(Constants.ShortCodeRegularExpression)
                .WithErrorCode(ErrorCode.InvalidUrl.Code)
                .WithMessage("The endpoint URL is not a valid URL");
        }

        public static async Task ValidateModelAsync(ShortLinkUpdateDto model)
        {
            var errorsList = new List<ErrorCode>();
            var val = new ShortLinkUpdateValidator();
            var res = await val.ValidateAsync(model);
            if (!res.IsValid)
            {
                foreach (var err in res.Errors)
                {
                    var errorCode = ErrorCode.All.FirstOrDefault(ec => ec.Code == err.ErrorCode);
                    errorsList.Add(errorCode);
                }

                throw new ModelValidationException(errorsList);
            }
        }
    }
}