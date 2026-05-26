using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net;
using BookingHotel.Application.Interfaces.Services;

namespace BookingHotel.Infrastructure.Services;

public class VNPayService : IVNPayService
{
    private readonly IConfiguration _config;

    public VNPayService(IConfiguration config) => _config = config;

    public string CreatePaymentUrl(decimal amount, string bookingCode, string returnUrl, string ipAddress)
    {
        var vnp_TmnCode = _config["PaymentSettings:VnPay:TmnCode"] ?? "MOCK_TMN";
        var vnp_HashSecret = _config["PaymentSettings:VnPay:HashSecret"] ?? "MOCK_SECRET";
        var vnp_Url = _config["PaymentSettings:VnPay:Url"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";

        var sortedList = new SortedList<string, string>
        {
            { "vnp_Version", "2.1.0" },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", vnp_TmnCode },
            { "vnp_Amount", ((long)(amount * 100)).ToString() },
            { "vnp_CurrCode", "VND" },
            { "vnp_TxnRef", bookingCode },
            { "vnp_OrderInfo", "Thanh toan don hang " + bookingCode },
            { "vnp_OrderType", "other" },
            { "vnp_Locale", "vn" },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_IpAddr", ipAddress },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
        };

        var data = new StringBuilder();
        foreach (var kv in sortedList)
        {
            data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
        }
        
        string queryString = data.ToString().TrimEnd('&');
        string vnp_SecureHash = HmacSha512(vnp_HashSecret, queryString);
        
        return vnp_Url + "?" + queryString + "&vnp_SecureHash=" + vnp_SecureHash;
    }

    private string HmacSha512(string key, string inputData)
    {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }
        }
        return hash.ToString();
    }
}

public class MoMoService : IMoMoService
{
    // Simplified Mock for now as it requires complex RSA/AES
    public string CreatePaymentUrl(decimal amount, string bookingCode, string returnUrl)
    {
        return $"https://test-payment.momo.vn/pay?amount={amount}&orderId={bookingCode}&returnUrl={returnUrl}&signature=MOCK_SIG";
    }
}
