public record class GetTransactionsDto(
        int Id,
        int ProjectId,
        int WalletId,
        string PaymentMethod,
        string PaymentType,
        DateTime CreatedAt
);