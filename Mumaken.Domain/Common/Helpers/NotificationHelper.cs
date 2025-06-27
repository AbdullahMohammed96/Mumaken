using AAITHelper.ModelHelper;
using Mumaken.Domain.Entities.UserTables;
using Nancy.Json;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;

namespace Mumaken.Domain.Common.Helpers
{
    public class NotificationHelper
    {
        public static async Task<bool> SendPushNotification(string applicationID_, string senderId_, List<DeviceIdModel> device_Ids, dynamic info, string msg = "", int? type = 0, int orderId = 0, int status = 0, string sound = "default")
        {
            foreach (DeviceIdModel device_Id in device_Ids)
            {
                if (device_Id is null) continue;

                string deviceId = device_Id.DeviceId;
                var message = new Message
                {
                    Notification = new Notification
                    {
                        Title = device_Id.ProjectName,
                        Body = msg
                    },

                    Token = deviceId,
                    Data = new Dictionary<string, string>
                    {
                        ["type"] = type.HasValue ? type.Value.ToString() : string.Empty,
                        ["orderId"] = orderId.ToString(),
                        ["status"] = status.ToString(),
                    }

                };

                try
                {
                    var messaging = FirebaseMessaging.DefaultInstance;
                    string response = await messaging.SendAsync(message);
                }
                catch (FirebaseMessagingException ex)
                {
                    continue;
                }
            }

            return true;

        }
    }
}
