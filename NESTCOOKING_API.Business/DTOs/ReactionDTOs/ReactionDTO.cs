using FluentValidation;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ReactionDTOs
{
    public class ReactionDTO
    {
        public StaticDetails.ReactionType Reaction { get; set; }
        public string TargetID { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
    public class ReactionDTOValidator : AbstractValidator<ReactionDTO>
    {
        public ReactionDTOValidator()
        {
            RuleFor(reaction => reaction.Reaction)
                .IsInEnum().WithMessage("Invalid reaction type.");

            RuleFor(reaction => reaction.TargetID)
                .NotEmpty().WithMessage("TargetID is required.");

            RuleFor(reaction => reaction.Type)
                .NotEmpty().WithMessage("Type is required.");
        }
    }
}
