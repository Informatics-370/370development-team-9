namespace TrackwiseAPI.Models.Entities
{
    public class CardPayment
    {
        public bool Completed { get; set; }
        public string Secure3DHtml { get; set; }
        public string PayRequestId { get; set; }
        public string Response { get; set; }
        public PaymentStatus Status { get; set; }
    }

/*    public class CardPayment
    {
        public bool Completed { get; set; }
        public string Secure3DHtml { get; set; }
        public string PayRequestId { get; set; }

    }
*/
    public class PaymentStatus
    {
        public string TransactionId { get; set; }
        public string Reference { get; set; }
        public string AcquirerCode { get; set; }
        public string StatusName { get; set; }
        public string AuthCode { get; set; }
        public string PayRequestId { get; set; }
        public string VaultId { get; set; }
        public List<PayVaultData> PayVaultData { get; set; }
        public string TransactionStatusCode { get; set; }
        public string TransactionStatusDescription { get; set; }
        public string ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string RiskIndicator { get; set; }
        public Payment_Type PaymentType { get; set; }
    }

    public class PayVaultData
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Payment_Type
    {
        public string Method { get; set; }
        public string Detail { get; set; }
    }

}
