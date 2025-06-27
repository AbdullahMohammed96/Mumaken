using AAITHelper.Enums;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.ChatDTO;
using Mumaken.Domain.Entities.Chat;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Chat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mumaken.Domain.Enums.ChatMessage;

namespace Mumaken.Service.Api.Implementation.Chat
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewMessage(string SenderId, string ReceiverId, int OrderId, string Text, int Type = 0, int Duration = 0)
        {
            try
            {
                Messages newmessage = new Messages();
                newmessage.SenderId = SenderId;
                newmessage.ReceiverId = ReceiverId;
                newmessage.OrderId = OrderId;
                newmessage.Text = Text;
                newmessage.TypeMessage = Type;
                newmessage.Duration = Duration;

                newmessage.DateSend = DateTime.UtcNow.AddHours(3);
                newmessage.IsDeletedReceiver = false;
                newmessage.IsDeletedSender = false;
                newmessage.LastMessage = LastMessage.LastMessage.ToNumber();
                await _context.Messages.AddAsync(newmessage);
                int result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<string>> GetDevicesId(string UserId)
        {
            List<string> Devices = await _context.DeviceIds.Where(x => x.UserId == UserId && x.DeviceId_ != null).Select(x => x.DeviceId_).AsNoTracking().ToListAsync();
            return Devices;
        }

        public async Task<List<ListMessageTwoUsersDto>> GetListMessageTwoUsersDto(string UserId, int OrderId, int pageNumber)
        {
            const int maxRows = 100;

            List<ListMessageTwoUsersDto> ListMessages = await _context.Messages.Where(x => x.OrderId == OrderId)
                .OrderByDescending(x => x.Id).Skip((pageNumber - 1) * maxRows).Take(maxRows)
               .Select(x => new ListMessageTwoUsersDto
               {
                   Id = x.Id,
                   Type = x.TypeMessage,
                   SenderId = x.SenderId,
                   ReceiverId = x.ReceiverId,
                   Message = x.Text,
                   Date = x.DateSend.ToString("hh:mm tt"),
                   OrderId = x.OrderId,
                   Duration = x.Duration
               }).OrderBy(x => x.Id).AsNoTracking().ToListAsync();

            return ListMessages;
        }

        //public async Task<List<ListUsersMyChatDto>> GetListUsersMyChat(string UserId, string lang)
        //{
        //    List<ListUsersMyChatDto> ListUsers = await (from order in _context.Orders
        //                                                //where order.Messages.Any(m => m.SenderId == UserId || m.ReceiverId == UserId)
        //                                                //let message = order.Messages.OrderByDescending(m => m.Id).FirstOrDefault()
        //                                                select new ListUsersMyChatDto
        //                                                {
        //                                                    Id = message.Id,
        //                                                    OrderNumber = message.OrderId,
        //                                                    lastMsg = message.Text,
        //                                                    UserId = message.SenderId == UserId ? message.ReceiverId : message.SenderId,
        //                                                    UserImg = message.SenderId == UserId ? DashBordUrl.baseUrlHost + message.Receiver.ImgProfile : DashBordUrl.baseUrlHost + message.Sender.ImgProfile,
        //                                                    UserName = message.SenderId == UserId ? message.Receiver.user_Name : message.Sender.user_Name,
        //                                                    Date = message.DateSend.ToString("dd/MM/yyyy")
        //                                                }).OrderByDescending(o => o.OrderNumber).AsNoTracking().ToListAsync();

        //    return ListUsers;
        //}

        public async Task<string> GetReceiverName(string UserId)
        {
            string ReceiverName = await _context.Users.Where(x => x.Id == UserId).Select(x => x.user_Name).FirstOrDefaultAsync();
            return ReceiverName;
        }
        public async Task<string> GetReceiverImg(string UserId)
        {
            string Img = await _context.Users.Where(x => x.Id == UserId).Select(x => x.ImgProfile).FirstOrDefaultAsync();
            return Img;
        }
    }
}
