using FluentValidation;
using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.TransactionDTOs
{
	public class TransactionDTO
	{
		public string Id { get; set; } = null!;
		public UserShortInfoDTO User { get; set; }
		public string Type { get; set; } = null!;
		public double Amount { get; set; }
		public string Description { get; set; } = null!;
		public string Currency { get; set; } = null!;
		public string Payment { get; set; } = null!;
        public bool IsSuccess { get; set; }
        public DateTime CreatedAt { get; set; }
	}
    public class TransactionDTOValidator : AbstractValidator<TransactionDTO>
    {
        public TransactionDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Transaction Id must not be empty.");

            RuleFor(x => x.User)
                .NotNull().WithMessage("User information must be provided.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Transaction type must not be empty.")
                .Must(type =>
                    type == "DEPOSIT" ||
                    type == "WITHDRAW" ||
                    type == "BOOKING" ||
                    type == "PURCHASEDRECIPE")
                .WithMessage("Transaction type must be one of the following: DEPOSIT, WITHDRAW, BOOKING, PURCHASEDRECIPE.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description must not be empty.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency must not be empty.");

            RuleFor(x => x.Payment)
                .NotEmpty().WithMessage("Payment information must not be empty.");

            RuleFor(x => x.IsSuccess)
                .NotNull().WithMessage("IsSuccess flag must not be null.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Creation date must not be empty.");
        }
    }
}
