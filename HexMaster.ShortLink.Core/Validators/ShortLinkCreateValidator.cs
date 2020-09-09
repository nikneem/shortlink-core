using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using HexMaster.ShortLink.Core.Enums;
using HexMaster.ShortLink.Core.Exceptions;
using HexMaster.ShortLink.Core.Models.ShortLinks;

namespace HexMaster.ShortLink.Core.Validators
{
    public class ShortLinkCreateValidator : AbstractValidator<ShortLinkCreateDto>

    {

        public ShortLinkCreateValidator()
        {
            RuleFor(x => x.EndpointUrl)
                .NotEmpty()
                .Matches(Constants.UrlRegularExpression)
                .WithErrorCode(ErrorCode.InvalidUrl.Code)
                .WithMessage("The endpoint URL is not a valid URL");
        }

        public static async Task ValidateModelAsync(ShortLinkCreateDto model)
        {
            var errorsList = new List<ErrorCode>();
            var val = new ShortLinkCreateValidator();
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