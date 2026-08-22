using System;
using FluentValidation;
using Patholab.Shared.DTOs;

namespace Patholab.Application.Validators
{
    public class PatientDtoValidator : AbstractValidator<PatientDto>
    {
        public PatientDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required.");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of birth cannot be in the future.");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^[6-9]\d{9}$").WithMessage("Mobile must be a valid 10-digit Indian number.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("A valid email address is required.").When(x => !string.IsNullOrEmpty(x.Email));
        }
    }

    public class DoctorDtoValidator : AbstractValidator<DoctorDto>
    {
        public DoctorDtoValidator()
        {
            RuleFor(x => x.DoctorName).NotEmpty().WithMessage("Doctor name is required.")
                .MaximumLength(150).WithMessage("Doctor name cannot exceed 150 characters.");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^[6-9]\d{9}$").WithMessage("Mobile must be a valid 10-digit Indian number.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("A valid email address is required.").When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.CommissionValue).GreaterThanOrEqualTo(0).WithMessage("Commission value must be 0 or positive.");
            RuleFor(x => x.CommissionValue).LessThanOrEqualTo(100).WithMessage("Commission percentage cannot exceed 100%.").When(x => x.CommissionType == Domain.Enums.CommissionType.Percentage);
        }
    }

    public class TestDtoValidator : AbstractValidator<TestDto>
    {
        public TestDtoValidator()
        {
            RuleFor(x => x.TestCode).NotEmpty().WithMessage("Test code is required.")
                .MaximumLength(20).WithMessage("Test code cannot exceed 20 characters.");
            RuleFor(x => x.TestName).NotEmpty().WithMessage("Test name is required.")
                .MaximumLength(150).WithMessage("Test name cannot exceed 150 characters.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be 0 or positive.");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("Valid department is required.");
            RuleFor(x => x.SampleTypeId).GreaterThan(0).WithMessage("Valid sample type is required.");
        }
    }

    public class TestParameterDtoValidator : AbstractValidator<TestParameterDto>
    {
        public TestParameterDtoValidator()
        {
            RuleFor(x => x.ParameterCode).NotEmpty().WithMessage("Parameter code is required.")
                .MaximumLength(50).WithMessage("Parameter code cannot exceed 50 characters.");
            RuleFor(x => x.ParameterName).NotEmpty().WithMessage("Parameter name is required.")
                .MaximumLength(150).WithMessage("Parameter name cannot exceed 150 characters.");
            RuleFor(x => x.ResultType).NotEmpty().WithMessage("Result type is required.");
        }
    }

    public class TestPackageDtoValidator : AbstractValidator<TestPackageDto>
    {
        public TestPackageDtoValidator()
        {
            RuleFor(x => x.PackageCode).NotEmpty().WithMessage("Package code is required.")
                .MaximumLength(20).WithMessage("Package code cannot exceed 20 characters.");
            RuleFor(x => x.PackageName).NotEmpty().WithMessage("Package name is required.")
                .MaximumLength(150).WithMessage("Package name cannot exceed 150 characters.");
            RuleFor(x => x.PackagePrice).GreaterThanOrEqualTo(0).WithMessage("Package price must be 0 or positive.");
        }
    }

    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0).WithMessage("Valid patient ID is required.");
            RuleFor(x => x.DiscountAmount).GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");
            RuleFor(x => x.PaidAmount).GreaterThanOrEqualTo(0).WithMessage("Paid amount cannot be negative.");
            RuleCustom();
        }

        private void RuleCustom()
        {
            RuleFor(x => x).Must(x => x.TestIds.Count > 0 || x.PackageIds.Count > 0)
                .WithMessage("At least one test or package must be selected for the order.");
        }
    }

    public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
    {
        public CreatePaymentRequestValidator()
        {
            RuleFor(x => x.InvoiceId).GreaterThan(0).WithMessage("Valid invoice ID is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Payment amount must be greater than zero.");
            RuleFor(x => x.PaymentMode).NotEmpty().WithMessage("Payment mode is required.");
        }
    }

    public class RequestHomeCollectionRequestValidator : AbstractValidator<RequestHomeCollectionRequest>
    {
        public RequestHomeCollectionRequestValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0).WithMessage("Valid patient ID is required.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Pincode).NotEmpty().WithMessage("Pincode is required.")
                .Matches(@"^\d{6}$").WithMessage("Pincode must be exactly 6 digits.");
            RuleFor(x => x.RequestedDate).NotEmpty().WithMessage("Requested date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Requested date cannot be in the past.");
            RuleFor(x => x.RequestedTime).NotEmpty().WithMessage("Requested time slot is required.");
        }
    }

    public class RejectSampleRequestValidator : AbstractValidator<RejectSampleRequest>
    {
        public RejectSampleRequestValidator()
        {
            RuleFor(x => x.SampleId).GreaterThan(0).WithMessage("Valid sample ID is required.");
            RuleFor(x => x.RejectionReason).NotEmpty().WithMessage("Rejection reason is required.")
                .MaximumLength(250).WithMessage("Rejection reason cannot exceed 250 characters.");
        }
    }
}
