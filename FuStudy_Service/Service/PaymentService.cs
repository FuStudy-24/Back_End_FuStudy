using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Net.payOS;
using Net.payOS.Types;
namespace FuStudy_Service.Service;

public class PaymentService
{
    private readonly PayOS _payOS;

    public PaymentService(string clientId, string apiKey, string checksumKey)
    {
        _payOS = new PayOS(clientId, apiKey, checksumKey);
    }

    public async Task<CreatePaymentResult> CreatePaymentLink(int orderId, int amount, string description)
    {
        ItemData item = new ItemData("Mì tôm hảo hảo ly", 1, 1000);
        List<ItemData> items = new List<ItemData> { item };

        PaymentData paymentData = new PaymentData(
            orderId,
            amount,
            description,
            items,
            cancelUrl: "https://localhost:3002",
            returnUrl: "https://localhost:3002"
        );

        return await _payOS.createPaymentLink(paymentData);
    }
}