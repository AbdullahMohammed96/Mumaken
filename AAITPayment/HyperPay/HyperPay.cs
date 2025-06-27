using AAITPayment.HyperPay.Model;
using Mumaken.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AAITPayment.HyperPay
{
    public class HyperPay
    {
        private string LiveCheckoutUrl { get; set; } = "https://oppwa.com/v1/checkouts";
        private string TestCheckoutUrl { get; set; } = "https://test.oppwa.com/v1/checkouts";

        private const string entityId = "entityId";
        private const string accessToken = "accessToken";
        private const string isLive = "isLive";

        public PaymentResult PayRequest(PaymentModel paymentModel, IHttpContextAccessor httpContextAccessor)
        {
            Dictionary<string, dynamic> responseData;
            decimal amountParsed = decimal.Parse(paymentModel.amount);
            string data;
            if (paymentModel.IsLive)
            {


                data = "entityId=" + (paymentModel.IsMada ? paymentModel.MadaEntityId : paymentModel.ViMaEntityId) +
                             "&amount=" + amountParsed.ToString("0.00") +
                             "&currency=" + paymentModel.currency +
                             "&paymentType=" + paymentModel.paymentType +
                             "&merchantTransactionId=" + Guid.NewGuid().ToString() +
                             "&billing.street1=" + paymentModel.street1 +
                             "&billing.city=" + paymentModel.city +
                             "&billing.state=" + paymentModel.state +
                             "&billing.country=" + paymentModel.country + //  Alpha-2 codes
                             "&billing.postcode=" + paymentModel.postcode +
                             "&customer.email=" + paymentModel.email +
                             "&customer.givenName=" + paymentModel.givenName +
                             "&customer.surname=" + paymentModel.surname;
            }
            else
            {
                data = "entityId=" + (paymentModel.IsMada ? paymentModel.MadaEntityId : paymentModel.ViMaEntityId) +
                            "&amount=" + amountParsed.ToString("0.00") +
                            "&currency=" + paymentModel.currency +
                            "&paymentType=" + paymentModel.paymentType +
                              (paymentModel.IsMada ? "&testMode=" + TestMode.INTERNAL : "&testMode=" + TestMode.EXTERNAL) +
                            "&merchantTransactionId=" + Guid.NewGuid().ToString() +
                            "&billing.street1=" + paymentModel.street1 +
                            "&billing.city=" + paymentModel.city +
                            "&billing.state=" + paymentModel.state +
                            "&billing.country=" + paymentModel.country + //  Alpha-2 codes
                            "&billing.postcode=" + paymentModel.postcode +
                            "&customer.email=" + paymentModel.email +
                            "&customer.givenName=" + paymentModel.givenName +
                            "&customer.surname=" + paymentModel.surname;

            }

            string url = paymentModel.IsLive ? LiveCheckoutUrl : TestCheckoutUrl;
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = paymentModel.IsLive ? "Bearer " + paymentModel.LiveAccessToken : "Bearer " + paymentModel.TestAccessToken;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reader.ReadToEnd());
                reader.Close();
                dataStream.Close();
            }

            PaymentResult paymentResult = new PaymentResult()
            {
                checkOutId = responseData["id"],
                userId = paymentModel.UserId,
                paymentMethod = paymentModel.PaymentMethod,
                isLive = paymentModel.IsLive

            };
            httpContextAccessor.HttpContext.Session.SetString(entityId, paymentModel.IsMada ? paymentModel.MadaEntityId : paymentModel.ViMaEntityId);
            httpContextAccessor.HttpContext.Session.SetString(accessToken, paymentModel.IsLive ? paymentModel.LiveAccessToken : paymentModel.TestAccessToken);
            httpContextAccessor.HttpContext.Session.SetInt32(isLive, paymentModel.IsLive ? 1 : 0);

            return paymentResult;
        }

        /// <summary>
        /// you need to pass checkout id to get the result
        /// </summary>
        /// <param name="checkOutId"></param>
        /// <returns>bool true / false</returns>
        public bool GetPaymentResult(PaymentResult resultPayment, IHttpContextAccessor httpContextAccessor)
        {
            string entityIdVal = httpContextAccessor.HttpContext.Session.GetString(entityId);
            string accessTokenVal = httpContextAccessor.HttpContext.Session.GetString(accessToken);
            int? isLiveVal = httpContextAccessor.HttpContext.Session.GetInt32(isLive);
            Dictionary<string, dynamic> responseData;
            string data = "entityId=" + entityIdVal;
            string url = isLiveVal == 1 ? LiveCheckoutUrl + "/" + resultPayment.checkOutId + "/payment?" + data
                                        : TestCheckoutUrl + "/" + resultPayment.checkOutId + "/payment?" + data;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + accessTokenVal;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reader.ReadToEnd());
                reader.Close();
                dataStream.Close();
            }
            var resultCode = responseData["result"]["code"].ToString();
            Regex successPattern = new Regex(@"(000\.000\.|000\.100\.1|000\.[36])");
            Regex successManuelPattern = new Regex(@"(000\.400\.0[^3]|000\.400\.100)");

            Match match1 = successPattern.Match(resultCode);
            Match match2 = successManuelPattern.Match(resultCode);

            if (match1.Success || match2.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}

