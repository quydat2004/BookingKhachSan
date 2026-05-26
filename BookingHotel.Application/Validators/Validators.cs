using FluentValidation;
using BookingHotel.Application.Features.Auth.Commands.Register;
using BookingHotel.Application.Features.Auth.Commands.Login;
using BookingHotel.Application.Features.Hotels.Commands.CreateHotel;
using BookingHotel.Application.Features.Rooms.Commands.CreateRoomType;
using BookingHotel.Application.Features.Reviews.Commands.CreateReview;
using BookingHotel.Application.Features.Payments.Commands.ProcessPayment;

namespace BookingHotel.Application.Validators;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(20);
    }
}

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class CreateHotelValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelValidator()
    {
        RuleFor(x => x.HotelName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StarRating).InclusiveBetween((byte)1, (byte)5);
    }
}

public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween((byte)1, (byte)5);
        RuleFor(x => x.Comment).MaximumLength(2000);
    }
}

public class CreateRoomTypeValidator : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeValidator()
    {
        RuleFor(x => x.TypeName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Capacity).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

public class ProcessPaymentValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0);
        RuleFor(x => x.PaymentMethod).NotEmpty().Must(m => new[] { "Cash", "VNPay", "MoMo", "CreditCard", "BankTransfer" }.Contains(m));
    }
}
