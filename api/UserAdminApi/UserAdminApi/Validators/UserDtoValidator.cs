using FluentValidation;
using UserAdminApi.Services;

namespace UserAdminApi.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty();
            RuleFor(dto => dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(dto => dto.Email).NotEmpty().MaximumLength(200).EmailAddress();
        }
    }

    public class UserCreateParamsValidator : AbstractValidator<UserCreateParams>
    {
        public UserCreateParamsValidator()
        {
            RuleFor(paramsObj => paramsObj.Name).NotEmpty().MaximumLength(200);
            RuleFor(paramsObj => paramsObj.Email).NotEmpty().MaximumLength(200).EmailAddress();
        }
    }
}
