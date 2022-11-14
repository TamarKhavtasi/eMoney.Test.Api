using EmoneyService;
using System.Security.Cryptography;
using System.Text;

namespace eMoney.Test.Api
{

    public interface IVendorService
    {

        Task<ResponseOfTransaction> PayAsync();
        Task<ResponseOfArrayOfParameter> GetInfoAsync();
        Task<ResponseOfTransaction> GetTransactionInfoAsync(string transactionCode);
    }
    public class VendorService : IVendorService
    {
        private const string Distributor = "TESTER";
        private const string Secret = "5PSSDCRF5GWMB2PULJ3H";
        private const string description = "Emoney";
        private const string serviceID = "1";
        private const string amount = "1.00";
        private const string currecny = "GEL";
        private const string transactionID = "5454";
        private const string customerCode = "360806773";

        public async Task<ResponseOfArrayOfParameter> GetInfoAsync()
        {
            using (var client = new ServiceClient())
            {
                var request = new Request();
                request.Distributor = Distributor;
                //var serviceID = "1";
                var customerCode = "360806773"; // ან მომხმარებლის ელფოსტა customerCode = " test@test.com";
                //var description = “Some Optional description, user can enter”;
                request.Hash = ComputeHash(string.Format("{0}{1}{2}{3}", "GetInfo", serviceID, Distributor, Secret));
                // Optional. თუ ეს პარამეტრი გადმოცემულია მოხდება ისეთი მომხმარებლების ძიება ვისაც აქვს eMoney-ში იგივე პირადობის ნომერი რაც შეყვანილი და მომხმარებელი ვერიფიცირებულია. სხვა შემთხვევაში ბრუნდება შეცდომა Code 47
                string verificationPinHash = ComputeHash(string.Format("{0}{1}{2}{3}", "GetInfo", serviceID, Distributor, Secret));
                request.Parameters = new Parameter[]
                {
                  //  new Parameter { Key = "verificationpin", Value = verificationPinHash },
                    new Parameter { Key = "service", Value = serviceID },
                    new Parameter { Key = "account", Value = customerCode },
                    new Parameter { Key = "description", Value = description }
                };

                return await client.GetInfoAsync(request);
                //return response;
            }


        }


        public async Task<ResponseOfTransaction> PayAsync()
        {
            using (var client = new ServiceClient())
            {
                var request = new Request();
                request.Distributor = Distributor;

                request.Hash = ComputeHash(string.Format("{0}{1}{2}{3}{4}{5}{6}","Pay", serviceID, amount, currecny, transactionID, Distributor, Secret));
                

                request.Parameters = new Parameter[]
                {
                    new Parameter {Key = "amount", Value = amount },
                    new Parameter {Key = "currency", Value = currecny },
                    new Parameter {Key = "service", Value = serviceID },
                    new Parameter {Key = "transaction", Value = transactionID },
                    new Parameter {Key = "account", Value = customerCode }
                };

                return await client.PayAsync(request);
            }
        }

        public async Task<ResponseOfTransaction> GetTransactionInfoAsync(string transactionCode)
        {
            using (var client = new ServiceClient())
            {
                var request = new Request();
                request.Distributor = Distributor;

                request.Hash = ComputeHash(string.Format("{0}{1}{2}{3}", "GetTransactionInfo", transactionCode, Distributor, Secret));
               

                request.Parameters = new Parameter[]
                {
                    new Parameter { Key = "transaction", Value = transactionCode }
                };

                return await client.GetTransactionInfoAsync(request);

            }

        }

        static string ComputeHash(string randomString)
        {
            var crypt = SHA256.Create();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
