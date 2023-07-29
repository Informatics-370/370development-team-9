using Newtonsoft.Json.Linq;
using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IPaymentRepository
    {
        /// <summary>
        /// make a payment and tokenize card for future use
        /// </summary>
        /// <param name="card">bank card information</param>
        /// <returns>request response from Paygate</returns>
        Task<CardPayment> AddNewCard(NewCard card);
        /// <summary>
        /// get the vaulted/tokenized card information
        /// </summary>
        /// <param name="vaultId">acquired when a card is vaulted</param>
        /// <returns>vaulted card information</returns>
        Task<JToken?> GetVaultedCard(string vaultId);
        /// <summary>
        /// query the status of a transaction using its id
        /// </summary>
        /// <param name="payRequestId">pay request id issued by Paygate when a payment requested is initialed</param>
        /// <returns>payment status information</returns>
        Task<JToken?> QueryTransaction(string payRequestId);
    }
}
