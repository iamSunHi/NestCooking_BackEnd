using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.TransactionDTOs
{
    public class TransactionInforValidatorTest
    {
        private readonly TransactionInforValidator _validator;
        private readonly TransactionInfor _transactionInfor;

        public TransactionInforValidatorTest()
        {
            _validator = new TransactionInforValidator();
            _transactionInfor = new TransactionInfor
            {
                OrderType = "DEPOSIT",
                Amount = 100.0,
                OrderDescription = "Order description",
                Name = "John Doe"
            };
        }

        [Fact]
        public void Should_Return_True_When_TransactionInfor_Is_Valid()
        {
            var result = _validator.Validate(_transactionInfor);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("INVALID_TYPE")]
        public void Should_Return_False_When_OrderType_Is_Invalid(string orderType)
        {
            _transactionInfor.OrderType = orderType;
            var result = _validator.Validate(_transactionInfor);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("DEPOSIT")]
        [InlineData("WITHDRAW")]
        [InlineData("BOOKING")]
        [InlineData("PURCHASEDRECIPE")]
        public void Should_Return_True_When_OrderType_Is_Valid(string orderType)
        {
            _transactionInfor.OrderType = orderType;
            var result = _validator.Validate(_transactionInfor);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Should_Return_False_When_Amount_Is_Invalid(double amount)
        {
            _transactionInfor.Amount = amount;
            var result = _validator.Validate(_transactionInfor);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_OrderDescription_Is_Invalid(string orderDescription)
        {
            _transactionInfor.OrderDescription = orderDescription;
            var result = _validator.Validate(_transactionInfor);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Name_Is_Invalid(string name)
        {
            _transactionInfor.Name = name;
            var result = _validator.Validate(_transactionInfor);
            Assert.False(result.IsValid);
        }
    }


}
