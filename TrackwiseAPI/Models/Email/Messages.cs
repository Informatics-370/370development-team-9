using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrackwiseAPI.Models.Email
{
    /*
    public class Messages
    {
        static void Main(string[] args)
        {
            var accountSid = "ACe2eb156857d631237dba578bdba5d2c8";
            var authToken = "642e2096998516038a0be6f9aea1809e";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("+27761532265"));
            messageOptions.From = new PhoneNumber("+12568587636");
            messageOptions.Body = "This message is sent by twilio";


            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
        }
    }
    */
}
