using FluentValidation;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.TransactionDTOs
{
    public class TransactionInfor
    {
        public string OrderType { get; set; } = null!;
        public double Amount { get; set; }
        public string OrderDescription { get; set; } = null!;
        public string Name { get; set; } = null!;

    }
    public class TransactionInforValidator : AbstractValidator<TransactionInfor>
    {
        public TransactionInforValidator()
        {
            RuleFor(x => x.OrderType)
                .NotEmpty().WithMessage("OrderType is required.")
                .Must(orderType => orderType == "DEPOSIT" ||
                                   orderType == "WITHDRAW" ||
                                   orderType == "BOOKING" ||
                                   orderType == "PURCHASEDRECIPE")
                .WithMessage("OrderType must be one of the following: DEPOSIT, WITHDRAW, BOOKING, PURCHASEDRECIPE.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.OrderDescription)
                .NotEmpty().WithMessage("OrderDescription is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}
