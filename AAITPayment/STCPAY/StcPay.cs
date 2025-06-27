using AAITHelper;
using AAITPayment.STCPAY.Model;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.STCPAY
{
    public class StcPay
    {
        // اول حاجة محتاجها شهادة ssl
        // محتاج برضو ملف pfx بيطلع من شهادة ssl - دي تتحط rootف المشروع
        // محتاج برضو MID (X-ClientCode) بيوصل من الايمل الخاص ب stc
        public string MID { get; set; }
        // ssl.pfx
        public string PfxCertificateName { get; set; }
        public string TestDirectPaymentAuthorizeUrl { get; set; } = "https://test.b2b.stcpay.com.sa:443/B2B.DirectPayment.WebApi/DirectPayment/V4/DirectPaymentAuthorize";
        public string TestDirectPaymentConfirmUrl { get; set; } = "https://test.b2b.stcpay.com.sa:443/B2B.DirectPayment.WebApi/DirectPayment/V4/DirectPaymentConfirm";
        public string LiveDirectPaymentAuthorizeUrl { get; set; }
        public string LiveDirectPaymentConfirmUrl { get; set; }
        public bool IsLive { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public StcPay()
        {

        }

        public StcPay(string mid, string pfxCertificateName, string testAuthUrl, string testConfirmUrl, string liveAuthUrl, string liveConfirmUrl, bool isLive, IWebHostEnvironment hostEnvironment)
        {
            MID = mid;
            PfxCertificateName = pfxCertificateName;
            TestDirectPaymentAuthorizeUrl = testAuthUrl;
            TestDirectPaymentConfirmUrl = testConfirmUrl;
            LiveDirectPaymentAuthorizeUrl = liveAuthUrl;
            LiveDirectPaymentConfirmUrl = liveConfirmUrl;
            IsLive = isLive;
            WebHostEnvironment = hostEnvironment;
        }

        public MainDirectPaymentAuthorizeV4ResponseMessage DirectPaymentAuthorize(DirectAuthorizePaymentModel directAuthorizePaymentModel)
        {

            try
            {
                MainDirectPaymentAuthorizeV4RequestMessage mainDirectPaymentAuthorizeV4RequestMessage = new MainDirectPaymentAuthorizeV4RequestMessage
                {
                    DirectPaymentAuthorizeV4RequestMessage = new DirectPaymentAuthorizeV4RequestMessage
                    {
                        Amount = directAuthorizePaymentModel.Amount,
                        BillNumber = "bill-" + HelperNumber.GetRandomNumber(),
                        BranchID = "branch-" + HelperNumber.GetRandomNumber(),
                        DeviceId = "device-" + HelperNumber.GetRandomNumber(),
                        MerchantNote = directAuthorizePaymentModel.Note,
                        MobileNo = directAuthorizePaymentModel.PhoneNumber,
                        RefNum = Guid.NewGuid().ToString(),
                        TellerId = "teller-" + HelperNumber.GetRandomNumber()
                    }
                };

                MainDirectPaymentAuthorizeV4ResponseMessage responseData = null;
                var data = JsonConvert.SerializeObject(mainDirectPaymentAuthorizeV4RequestMessage);


                string url = IsLive ? LiveDirectPaymentAuthorizeUrl : TestDirectPaymentAuthorizeUrl;
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

                var pfxFilePath = Path.Combine(WebHostEnvironment.ContentRootPath, PfxCertificateName);

                var userCertificate = new X509Certificate2(pfxFilePath, "", X509KeyStorageFlags.MachineKeySet);
                request.Method = "POST";
                request.Headers["X-ClientCode"] = MID;
                request.ContentType = "application/json";
                request.ClientCertificates.Add(userCertificate);
                Stream PostData = request.GetRequestStream();
                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();



                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseData = JsonConvert.DeserializeObject<MainDirectPaymentAuthorizeV4ResponseMessage>(reader.ReadToEnd());
                    reader.Close();
                    dataStream.Close();
                }

                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MainDirectPaymentConfirmV4ResponseMessage DirectPaymentConfirm(DirectPaymentConfirmModel directPaymentConfirmModel)
        {

            try
            {
                MainDirectPaymentConfirmV4RequestMessage mainDirectPaymentConfirmV4RequestMessage = new MainDirectPaymentConfirmV4RequestMessage
                {
                    DirectPaymentConfirmV4RequestMessage = new DirectPaymentConfirmV4RequestMessage
                    {
                        OtpReference = directPaymentConfirmModel.OtpReference,
                        OtpValue = directPaymentConfirmModel.OtpValue,
                        STCPayPmtReference = directPaymentConfirmModel.StcPayPmtReference,
                        TokenizeYn = "true",
                        TokenReference = Guid.NewGuid().ToString()
                    }
                };

                MainDirectPaymentConfirmV4ResponseMessage responseData = null;
                var data = JsonConvert.SerializeObject(mainDirectPaymentConfirmV4RequestMessage);


                string url = IsLive ? LiveDirectPaymentConfirmUrl : TestDirectPaymentConfirmUrl;
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

                var pfxFilePath = Path.Combine(WebHostEnvironment.ContentRootPath, PfxCertificateName);

                X509Certificate2 userCertificate = new X509Certificate2(pfxFilePath);
                request.Method = "POST";
                request.Headers["X-ClientCode"] = MID;
                request.ContentType = "application/json";
                request.ClientCertificates.Add(userCertificate);
                Stream PostData = request.GetRequestStream();
                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();



                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseData = JsonConvert.DeserializeObject<MainDirectPaymentConfirmV4ResponseMessage>(reader.ReadToEnd());
                    reader.Close();
                    dataStream.Close();
                }
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
