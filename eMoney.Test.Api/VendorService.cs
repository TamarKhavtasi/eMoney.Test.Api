using EmoneyService;
using System.Security.Cryptography;
using System.Text;

namespace eMoney.Test.Api
{
    public class VendorService
    {

        public async Task<ResponseOfArrayOfParameter> GetInfoAsync()
        {
            using (var client = new ServiceClient())
            {
                var request = new Request();
                request.Distributor = "DISTRIBUTOR";
                var serviceID = "1";
                var customerCode = " 369362582"; // ან მომხმარებლის ელფოსტა customerCode = " test@test.com";
                //var description = “Some Optional description, user can enter”;
                request.Hash = string.Format("{0}{1}{2}{3}", "GetInfo", serviceID, "DISTRIBUTOR", "SECRET_KEY");
                // Optional. თუ ეს პარამეტრი გადმოცემულია მოხდება ისეთი მომხმარებლების ძიება ვისაც აქვს eMoney-ში იგივე პირადობის ნომერი რაც შეყვანილი და მომხმარებელი ვერიფიცირებულია. სხვა შემთხვევაში ბრუნდება შეცდომა Code 47
                string verificationPinHash = string.Format("{0}{1}{2}{3}", "GetInfo", "user_private_numer_from_your_system", "DISTRIBUTOR", "SECRET_KEY");
                request.Parameters = new Parameter[]
                {
                    new Parameter { Key = "verificationpin", Value = verificationPinHash },
                    new Parameter { Key = "service", Value = serviceID },
                    new Parameter { Key = "account", Value = customerCode },
                    //new Parameter { Key = "description", Value = description }
                };

                return await client.GetInfoAsync(request);
                //return response;
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
