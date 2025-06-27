using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.UserTables;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mumaken.Domain.Entities.Chat
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime DateSend { get; set; }
        public bool SenderSeen { get; set; }
        public bool ReceiverSeen { get; set; }
        public bool IsDeletedSender { get; set; }
        public bool IsDeletedReceiver { get; set; }
        public int LastMessage { get; set; }
        public int Duration { get; set; }

        public int TypeMessage { get; set; }

        /****************************************************************/
        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationDbUser Sender { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public virtual ApplicationDbUser Receiver { get; set; }
        [ForeignKey(nameof(OrderId))]
        /****************************************************************/
        public virtual Order Order { get; set; }

    }
}
