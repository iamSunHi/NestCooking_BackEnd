using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.ReportDTOs
{
    public class UpdateReportDTOValidatorTest
    {
        private readonly UpdateReportDTOValidator _validator;
        private readonly UpdateReportDTO _updateReportDTO;

        public UpdateReportDTOValidatorTest()
        {
            _validator = new UpdateReportDTOValidator();
            _updateReportDTO = new UpdateReportDTO
            {
                Id = "123",
                Title = "Update Report",
                Content = "This is the updated content.",
                ImagesURL = "https://example.com/image.jpg"
            };
        }

        [Fact]
        public void Should_Return_True_When_Valid_UpdateReportDTO()
        {
            var result = _validator.Validate(_updateReportDTO);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Id_Is_Null_Or_Empty(string id)
        {
            _updateReportDTO.Id = id;
            var result = _validator.Validate(_updateReportDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Title_Is_Null_Empty_Or_Short(string title)
        {
            _updateReportDTO.Title = title;
            var result = _validator.Validate(_updateReportDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Content_Is_Null_Or_Empty(string content)
        {
            _updateReportDTO.Content = content;
            var result = _validator.Validate(_updateReportDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("invalid-url", false)]
        [InlineData("https://example.com/image.jpg", true)]
        public void Should_Return_Expected_Result_Based_On_ImagesURL(string imagesURL, bool expectedIsValid)
        {
            _updateReportDTO.ImagesURL = imagesURL;
            var result = _validator.Validate(_updateReportDTO);
            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Fact]
        public void Should_Return_True_When_ImagesURL_Is_Null()
        {
            _updateReportDTO.ImagesURL = null;
            var result = _validator.Validate(_updateReportDTO);
            Assert.True(result.IsValid);
        }
    }
}
