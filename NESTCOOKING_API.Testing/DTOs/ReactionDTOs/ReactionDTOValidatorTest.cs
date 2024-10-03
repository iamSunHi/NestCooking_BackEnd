using NESTCOOKING_API.Business.DTOs.ReactionDTOs;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.ReactionDTOs
{
    public class ReactionDTOValidatorTest
    {
        private readonly ReactionDTOValidator _validator;
        private readonly ReactionDTO _reactionDTO;

        public ReactionDTOValidatorTest()
        {
            _validator = new ReactionDTOValidator();
            _reactionDTO = new ReactionDTO
            {
                Reaction = StaticDetails.ReactionType.like,
                TargetID = "123",
                Type = "Comment"
            };
        }

        [Fact]
        public void Should_Return_True_When_Valid_ReactionDTO()
        {
            var result = _validator.Validate(_reactionDTO);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Return_False_When_TargetID_Is_Null_Or_Empty(string targetId)
        {
            _reactionDTO.TargetID = targetId;
            var result = _validator.Validate(_reactionDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Return_False_When_Type_Is_Null_Or_Empty(string type)
        {
            _reactionDTO.Type = type;
            var result = _validator.Validate(_reactionDTO);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Return_False_When_Invalid_ReactionType()
        {
            _reactionDTO.Reaction = (StaticDetails.ReactionType)999;
            var result = _validator.Validate(_reactionDTO);
            Assert.False(result.IsValid);
        }
    }
}
