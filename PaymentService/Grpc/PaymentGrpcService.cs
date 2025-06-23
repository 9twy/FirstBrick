using Grpc.Core;
using Protos.Grpc;
using RabbitMQ.Stream.Client;

public class PaymentGrpcService : Protos.Grpc.MakePaymentService.MakePaymentServiceBase
{
    private readonly IPaymentService _paymentServices;
    public PaymentGrpcService(IPaymentService paymentService)
    {
        _paymentServices = paymentService;
    }
    public override async Task<MakePaymentResponse> MakePayment(MakePaymentRequest request, ServerCallContext context)
    {
        var result = await _paymentServices.MakePaymentAsync
        (
            request.UserId,
            (decimal)request.Amount,
            request.ProjectId
        );
        return new MakePaymentResponse
        {
            Success = result,
            Message = result ? "Success" : "Infficent balance"
        };

    }

}