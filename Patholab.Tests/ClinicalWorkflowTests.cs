using Xunit;
using Patholab.Application.Common;
using Patholab.Domain.Enums;
using Patholab.Domain.Entities;
using Patholab.Application.Validators;
using Patholab.Shared.DTOs;

namespace Patholab.Tests
{
    public class ClinicalWorkflowTests
    {
        // ==========================================
        // 1. PASSWORD HASHER CRYPTOGRAPHY TESTS
        // ==========================================
        [Fact]
        public void PasswordHasher_ShouldHashAndVerifySuccessfully()
        {
            // Arrange
            var rawPassword = "SecurePassword@123";

            // Act
            var hash = PasswordHasher.HashPassword(rawPassword);
            var isCorrect = PasswordHasher.VerifyPassword(rawPassword, hash);
            var isIncorrect = PasswordHasher.VerifyPassword("WrongPassword", hash);

            // Assert
            Assert.True(isCorrect);
            Assert.False(isIncorrect);
        }

        // ==========================================
        // 2. CLINICAL RANGE FLAG CALCULATION TESTS
        // ==========================================
        [Theory]
        // Hemoglobin Adult Male (Ref: 13.0 - 17.0, Critical Low: 7.0, Critical High: 20.0)
        [InlineData("Male", 35, 14.5, ResultStatus.Normal)]
        [InlineData("Male", 35, 11.5, ResultStatus.Low)]
        [InlineData("Male", 35, 6.2, ResultStatus.Critical)]
        [InlineData("Male", 35, 22.0, ResultStatus.Critical)]
        // Hemoglobin Adult Female (Ref: 12.0 - 15.0)
        [InlineData("Female", 28, 13.2, ResultStatus.Normal)]
        [InlineData("Female", 28, 10.5, ResultStatus.Low)]
        public void CalculateResultStatus_ShouldAssignCorrectFlags(string gender, int age, decimal value, ResultStatus expectedStatus)
        {
            // Arrange
            var param = new TestParameter
            {
                ParameterCode = "HB",
                ParameterName = "Hemoglobin",
                ResultType = "Numeric",
                MaleMin = 13.0m,
                MaleMax = 17.0m,
                FemaleMin = 12.0m,
                FemaleMax = 15.0m,
                ChildMin = 11.0m,
                ChildMax = 14.0m,
                CriticalLow = 7.0m,
                CriticalHigh = 20.0m
            };

            // Act
            var status = CalculateResultStatus(param, value, gender, age);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        // ==========================================
        // 3. INPUT PAYLOAD VALIDATION PIPELINE TESTS
        // ==========================================
        [Fact]
        public void PatientValidator_ShouldDetectMissingNames()
        {
            // Arrange
            var validator = new PatientDtoValidator();
            var invalidDto = new PatientDto
            {
                FirstName = "", // Invalid
                LastName = "Doe",
                Mobile = "9876543210"
            };

            // Act
            var validationResult = validator.Validate(invalidDto);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, e => e.PropertyName == "FirstName");
        }

        // ==========================================
        // PRIVATE TEST HELPERS
        // ==========================================
        private static ResultStatus CalculateResultStatus(TestParameter param, decimal value, string gender, int age)
        {
            // Determine clinical reference ranges based on patient metrics
            decimal? min = param.MaleMin;
            decimal? max = param.MaleMax;

            if (age < 12)
            {
                min = param.ChildMin;
                max = param.ChildMax;
            }
            else if (gender == "Female")
            {
                min = param.FemaleMin;
                max = param.FemaleMax;
            }

            // Evaluate critical ranges
            if (param.CriticalLow.HasValue && value <= param.CriticalLow.Value)
            {
                return ResultStatus.Critical;
            }
            if (param.CriticalHigh.HasValue && value >= param.CriticalHigh.Value)
            {
                return ResultStatus.Critical;
            }

            // Evaluate normal ranges
            if (min.HasValue && value < min.Value)
            {
                return ResultStatus.Low;
            }
            if (max.HasValue && value > max.Value)
            {
                return ResultStatus.High;
            }

            return ResultStatus.Normal;
        }
    }
}
