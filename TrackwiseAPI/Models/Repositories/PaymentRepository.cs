using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Xml;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.Interfaces;
using RestSharp;

namespace TrackwiseAPI.Models.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly RestClient _client;
        public PaymentRepository()
        {
            _client = new RestClient("https://secure.paygate.co.za/payhost/process.trans");
        }
        public async Task<CardPayment> AddNewCard(NewCard card)
        {
            CardPayment payResponse = new CardPayment
            {
                Completed = false,
            };
            RestRequest request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "text/xml");
            request.AddHeader("SOAPAction", "WebPaymentRequest");

            // request body
            string body;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "Templates", "SinglePaymentRequest.xml");
            using (StreamReader reader = new StreamReader(filePath))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{PayGateId}", "");
            body = body.Replace("{Password}", "");
            body = body.Replace("{FirstName}", card.FirstName);
            body = body.Replace("{LastName}", card.LastName);
            body = body.Replace("{Mobile}", "");
            body = body.Replace("{Email}", card.Email);
            body = body.Replace("{CardNumber}", card.CardNumber.ToString());
            body = body.Replace("{CardExpiryDate}", card.CardExpiry.ToString());
            body = body.Replace("{CVV}", card.Cvv.ToString());
            // body = body.Replace("{Vault}", false.ToString());
            body = body.Replace("{MerchantOrderId}", Guid.NewGuid().ToString());
            // convert amount to cents (amount * 100)
            body = body.Replace("{Amount}", (card.Amount * 100).ToString("0000"));
            request.AddParameter("text/xml", body, ParameterType.RequestBody);
            RestResponse response = await _client.ExecuteAsync(request);

  
            string[] map = { "SinglePaymentResponse", "CardPaymentResponse" };
            JToken? result = MapXmlResponseToObject(response.Content, map);
            // check payment response
            if (result?["Status"] != null)
            {
                payResponse.Response = result?["Status"].ToString();
                payResponse.Status = JsonConvert.DeserializeObject<PaymentStatus>(payResponse.Response);
                payResponse.Response = JsonConvert.SerializeObject(result);
                JToken? paymentStatus = result["Status"];
                switch (paymentStatus?["StatusName"]?.ToString())
                {
                    case "Error":
                        throw new ApplicationException();

                    case "Completed" when paymentStatus?["ResultCode"] != null:
                        payResponse.Completed = true;
                        payResponse.PayRequestId = paymentStatus?["PayRequestId"]?.ToString();
                        payResponse.Secure3DHtml = null;
                        if (paymentStatus?["ResultCode"]?.ToString() == "990017")
                        {

                            return payResponse;
                        }
                        throw new ApplicationException($"{paymentStatus?["ResultCode"]}: Payment declined");

                    case "ThreeDSecureRedirectRequired":
                        // payment requires 3D verification
                        JToken? redirectXml = result["Redirect"];
                        if (redirectXml?["UrlParams"] != null)
                        {
                            RestClient client = new RestClient(redirectXml["RedirectUrl"]?.ToString()!);
                            JArray urlParams = JArray.Parse(redirectXml["UrlParams"]?.ToString()!);
                            Dictionary<string, string> urlParamsDictionary = urlParams.Cast<JObject>().ToDictionary(item => item.GetValue("key")?.ToString(),
                                    item => item.GetValue("value")?.ToString())!;
                            string httpRequest = ToUrlEncodedString(urlParamsDictionary!);

                            RestRequest req = new RestRequest("/api/Payment/AddNewCard", Method.Post);
                            req.AddParameter("application/x-www-form-urlencoded", httpRequest,
                                ParameterType.RequestBody);
                            RestResponse res = await client.ExecuteAsync(req);

                            if (!res.IsSuccessful) throw new ApplicationException(res.ErrorMessage);

                            string responseContent = res.Content;
                            payResponse.Completed = false;
                            payResponse.Secure3DHtml = responseContent;
                            payResponse.PayRequestId = urlParamsDictionary["PAY_REQUEST_ID"];
                            return payResponse;

                        }
                        break;
                }
            }
            throw new ApplicationException("Payment request returned no results");
        }

        public async Task<JToken?> GetVaultedCard(string vaultId)
        {
            RestRequest request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "text/xml");
            request.AddHeader("SOAPAction", "SingleVaultRequest");

            // request body
            string body;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "Templates", "SingleVaultRequest.xml");
            using (StreamReader reader = new StreamReader(filePath))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{PayGateId}", "");
            body = body.Replace("{Password}", "");
            body = body.Replace("{VaultId}", vaultId);
            request.AddParameter("text/xml", body, ParameterType.RequestBody);
            RestResponse response = await _client.ExecuteAsync(request);

            // example positive response
            /*{
              "SingleVaultResponse": {
                "@xmlns:ns2": "http://www.paygate.co.za/PayHOST",
                "LookUpVaultResponse": {
                  "Status": {
                    "StatusName": "Completed",
                    "PayVaultData": [
                      {
                        "name": "cardNumber",
                        "value": "520000xxxxxx0015"
                      },
                      {
                        "name": "expDate",
                        "value": "102023"
                      }
                    ],
                    "PaymentType": {
                      "Method": "CC",
                      "Detail": "MasterCard"
                    }
                  }
                }
              }
            }*/
            string[] map = { "SingleVaultResponse", "LookUpVaultResponse" };
            JToken? result = MapXmlResponseToObject(response.Content, map);
            return result;
        }

        public async Task<JToken?> QueryTransaction(string payRequestId)
        {
            RestRequest request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "text/xml");
            request.AddHeader("SOAPAction", "SingleFollowUpRequest");

            // request body
            string body;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "Templates", "SingleFollowUpRequest.xml");
            using (StreamReader reader = new StreamReader(filePath))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{PayGateId}", "");
            body = body.Replace("{Password}", "");
            body = body.Replace("{PayRequestId}", payRequestId);
            request.AddParameter("text/xml", body, ParameterType.RequestBody);
            RestResponse response = await _client.ExecuteAsync(request);

            /*
             {
                "SingleFollowUpResponse": {
                "@xmlns:ns2": "http://www.paygate.co.za/PayHOST",
                "QueryResponse": {
                  "Status": {
                    "TransactionId": "292777334",
                    "Reference": "55813452-ddb6-4cfd-bdd5-bdef07fdd2ea",
                    "AcquirerCode": "00",
                    "StatusName": "Completed",
                    "AuthCode": "JIUW72",
                    "PayRequestId": "4EE45210-F7E4-494C-82CD-6C2FB97F2102",
                    "VaultId": "8b1f081f-9bf0-4351-8403-0ff28e8fab36",
                    "PayVaultData": [
                      {
                        "name": "cardNumber",
                        "value": "xxxxxxxxxxxx0015"
                      },
                      {
                        "name": "expDate",
                        "value": "102023"
                      }
                    ],
                    "TransactionStatusCode": "1",
                    "TransactionStatusDescription": "Approved",
                    "ResultCode": "990017",
                    "ResultDescription": "Auth Done",
                    "Currency": "ZAR",
                    "Amount": "1530",
                    "RiskIndicator": "AP",
                    "PaymentType": {
                      "Method": "CC",
                      "Detail": "MasterCard"
                    },
                    "DateTime": "2021-06-14T15:03:25+02:00",
                    "TransactionType": "Authorisation"
                  }
                }
                }
                }
             */
            string[] map = { "SingleFollowUpResponse", "QueryResponse" };
            return MapXmlResponseToObject(response.Content, map);
        }

        private static JToken? MapXmlResponseToObject(string xmlContent, string[]? responseKeys)
        {
            XmlDocument xmlResult = new XmlDocument();
            // throws exception if it fails to parse xml
            xmlResult.LoadXml(xmlContent);
            // convert to json
            string result = JsonConvert.SerializeXmlNode(xmlResult);
            // remove prefix tags
            result = Regex.Replace(result, @"\bns2:\b", "");
            // parse as json object
            JObject paymentResponse = JObject.Parse(result);
            // return response
            JToken? response = paymentResponse["SOAP-ENV:Envelope"]?["SOAP-ENV:Body"];
            if (responseKeys != null)
            {
                response = responseKeys.Aggregate(response, (current, t) => current?[t]);
            }
            return response;
        }

        private static string ToUrlEncodedString(Dictionary<string, string?> request)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in request.Keys)
            {
                builder.Append("&");
                builder.Append(key);
                builder.Append("=");
                builder.Append(HttpUtility.UrlEncode(request[key]));
            }
            string result = builder.ToString().TrimStart('&');
            return result;
        }
    }
}
