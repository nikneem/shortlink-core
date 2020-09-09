using System.Text.RegularExpressions;
using FluentValidation;
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
                .WithMessage("The endpoint URL is not a valid URL");
        }

    }
}