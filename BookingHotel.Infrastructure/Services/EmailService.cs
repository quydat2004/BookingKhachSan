using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using BookingHotel.Application.Interfaces.Services;

namespace BookingHotel.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config) => _config = config;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Booking Hotel", _config["Email:From"]));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["Email:Host"], int.Parse(_config["Email:Port"]!), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendBookingConfirmationAsync(string email, string customerName, string bookingCode, string hotelName, DateTime checkIn, DateTime checkOut, decimal amount)
    {
        var body = $"""
        <h2>Booking Confirmed!</h2>
        <p>Dear {customerName},</p>
        <p>Your booking <strong>{bookingCode}</strong> at {hotelName} has been confirmed.</p>
        <p>Check-in: {checkIn:dd/MM/yyyy}</p>
        <p>Check-out: {checkOut:dd/MM/yyyy}</p>
        <p>Total: {amount:N0} VND</p>
        """;
        await SendEmailAsync(email, $"Booking Confirmed - {bookingCode}", body);
    }

    public async Task SendPaymentSuccessAsync(string email, string customerName, string bookingCode, decimal amount, string transactionId)
    {
        var body = $"""
        <h2>Payment Successful!</h2>
        <p>Dear {customerName},</p>
        <p>Payment for booking <strong>{bookingCode}</strong> has been processed.</p>
        <p>Amount: {amount:N0} VND</p>
        <p>Transaction: {transactionId}</p>
        """;
        await SendEmailAsync(email, $"Payment Success - {bookingCode}", body);
    }
}
