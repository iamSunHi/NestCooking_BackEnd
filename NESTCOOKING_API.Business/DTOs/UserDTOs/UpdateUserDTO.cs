using FluentValidation;

namespace NESTCOOKING_API.Business.DTOs.UserDTOs
{
	public class UpdateUserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public string Address { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDTOValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage("Username can not be null").MaximumLength(50);
            RuleFor(x => x.FirstName).NotNull().WithMessage("First name can not be null").MaximumLength(50);
            RuleFor(x => x.LastName).NotNull().WithMessage("Last name can not be null").MaximumLength(50);
            RuleFor(x => x.Address).NotNull().WithMessage("Address can not be null").MaximumLength(50);
            RuleFor(x => x.AvatarUrl).NotNull().WithMessage("Avatar can not be null").MaximumLength(50);
        }
    }
}
