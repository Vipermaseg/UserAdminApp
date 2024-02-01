using FluentValidation;
using UserAdminApi.Services;

namespace UserAdminApi.Validators
{
    public class UserCreateParamsValidator : AbstractValidator<UserCreateParams>
    {
        public UserCreateParamsValidator()
        {
            RuleFor(paramsObj => paramsObj.Name).NotEmpty().MaximumLength(200);
            RuleFor(paramsObj => paramsObj.Email).NotEmpty().MaximumLength(200).EmailAddress();
        }
    }
}
