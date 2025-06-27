using AAITPayment.HyperPay.Model;
using AAITPayment.MyFatoorah.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.MyFatoorah
{
    public class MyFatoorah
    {
        public MyFatoorahCallBackUrlConfig MyFatoorahCallBackUrlConfig { get; set; }
        //// live
        //token value to be placed here from merchant account;
        public string LiveToken { get; set; } = "";

        // test
        public string TestToken { get; set; } = "-Nb3Hjtgk-cHZFXFn3ukd9-CMGmYN2oFzjg4W0WNK4K7aYY20T0vyZWdGBHO-FrW-HSp4UCL0sqtDKK5NEX5dMq0KeIy2ySXBIagzj2wjVlRPrX1QFtBwAl3O-S2T4AX4ll-uyrsuvrHaYScN65txmh0FiW9bkYspO4H2Bpkkupv2mdG-BRvDeP8Lhdir0gPz65Jez8SAHKY8raSK2mTFK5CP7hUjdmpb_Iqz6yESpy4UV36Aj52LSleCPxlE3ySTAsp0kvAVGj-EqFzdp0BrAO-YK5AnEpr7_hbPwdTi1nisRwNBZYmjUXfCW2c_Yqzh1DHLEyxB4lD5d5LHziuNsw1du_WfhoXjJprf1-NriBh9nbtF8dvwXJR0GKk0_SSGdjwQ64Yz0mkt4kWkRekg2bbX7znq4QupQoMf7QMtGwkT5lBaE2SPhaUz78degNtSoLvxb_kUFxxOKGl1o1B37rsI7zIFpgrETEq-XQBTwCMyWfbUu36klAqQpIR3upKTxcaGsLGV7tvKSVifteEGy_qi1snpuOOe0IwQVntlj7U8JhlZQ36JjHMINmALPDA2YIqjm0cHt0YH9JQbwQvqjIPlVSAuP4C7iwI2lOtIBEVg7Rb4gsphBcX2afuxZWvIHiMztKk0jJDWZHXLM-Y-SrnyOv-AOkiQSCm2OeSNChoA5okIRrVA0UtfnV45d5PaCb3VA"; //token value to be placed here;
        public string liveBaseApiURL { get; set; } = "https://api-sa.myfatoorah.com/"; // ksa live
        public string testBaseApiURL { get; set; } = "https://apitest.myfatoorah.com/"; // test
        public bool IsLive { get; set; }



        public MyFatoorah()
        {

        }

        public MyFatoorah(MyFatoorahCallBackUrlConfig myFatoorahCallBackUrlConfig, RouteValues routeValues)
        {
            MyFatoorahCallBackUrlConfig = myFatoorahCallBackUrlConfig;
            myFatoorahCallBackUrlConfig.QueryParamsRouteValues = nameof(routeValues.OrderId) + "=" + routeValues.OrderId + "&" + nameof(routeValues.SubScriptionId) + "=" + routeValues.SubScriptionId + "&" + nameof(routeValues.Type) + "=" + routeValues.Type;
        }

        public List<PaymentMethod> InitiatePayment(string amount)
        {
            //####### Initiate Payment ######
            InitiatePayment initiatePayment = new InitiatePayment()
            {
                InvoiceAmount = amount,
                CurrencyIso = IsLive ? Currency.SAR : Currency.KWD
            };

            var initiatePaymentDictionary = initiatePayment
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(initiatePayment, null));


            string url = IsLive ? liveBaseApiURL + "/v2/InitiatePayment" : testBaseApiURL + "/v2/InitiatePayment";
            HttpClient client = new HttpClient();
            var token = IsLive ? LiveToken : TestToken;
            byte[] cred = Encoding.UTF8.GetBytes("Bearer " + token);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var parameters = new Dictionary<string, string> { { "InvoiceAmount", "100" }, { "CurrencyIso", "SAR" } };
            var parameters = initiatePaymentDictionary;
            var encodedContent = new FormUrlEncodedContent(parameters);
            HttpResponseMessage messge = client.PostAsync(url, encodedContent).Result;
            string initiateResult;
            InitiateResponse initiateResponse;
            if (messge.IsSuccessStatusCode)
            {
                initiateResult = messge.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                initiateResponse = JsonConvert.DeserializeObject<InitiateResponse>(initiateResult);
                //Response.Write("\n" + initiateResult + "\n");
            }
            else
            {
                initiateResult = messge.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                initiateResponse = JsonConvert.DeserializeObject<InitiateResponse>(initiateResult);
                //Response.Write("\n" + initiateResult + "\n");
            }

            var paymentMethods = new List<PaymentMethod>();

            if (initiateResponse.IsSuccess)
            {
                paymentMethods = initiateResponse.Data.PaymentMethods;
            }

            ////####### Execute Payment ######

            return paymentMethods;
        }

        public ExecutePaymentResponse ExecutePayment(ExecutePaymentModel executePaymentModel)
        {
            string url = IsLive ? liveBaseApiURL + "/v2/ExecutePayment" : testBaseApiURL + "/v2/ExecutePayment";
            var token = IsLive ? LiveToken : TestToken;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string json = "{\"PaymentMethodId\":\"2\",\"CustomerName\": \"Ahmed\",\"DisplayCurrencyIso\": \"KWD\",\"MobileCountryCode\":\"+965\",\"CustomerMobile\": \"12345678\",\"CustomerEmail\": \"xx@yy.com\",\"InvoiceValue\": 100,\"CallBackUrl\": \"https://google.com\",\"ErrorUrl\": \"https://google.com\",\"Language\": \"en\",\"CustomerReference\" :\"ref 1\",\"CustomerCivilId\":12345678,\"UserDefinedField\": \"Custom field\",\"ExpireDate\": \"\",\"CustomerAddress\" :{\"Block\":\"\",\"Street\":\"\",\"HouseBuildingNo\":\"\",\"Address\":\"\",\"AddressInstructions\":\"\"},\"InvoiceItems\": [{\"ItemName\": \"Product 01\",\"Quantity\": 1,\"UnitPrice\": 100}]}";


            string json = JsonConvert.SerializeObject(executePaymentModel);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var messge = client.PostAsync(url, content).Result;
            string executeResult;
            ExecutePaymentResponse resultResponse;
            if (messge.IsSuccessStatusCode)
            {
                executeResult = messge.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                resultResponse = JsonConvert.DeserializeObject<ExecutePaymentResponse>(executeResult);
                //Response.Write("\n" + executeResult + "\n");
            }
            else
            {
                executeResult = messge.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                resultResponse = JsonConvert.DeserializeObject<ExecutePaymentResponse>(executeResult);
                //Response.Write("\n" + executeResult + "\n");

            }

            return resultResponse;
        }

    }
}
