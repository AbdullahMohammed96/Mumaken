using Mumaken.Domain.Entities.UserTables;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mumaken.Domain.Entities.Chat
{
    public class ConnectUser
    {
        [Key]
        public int Id { get; set; }
        public string ContextId { get; set; }
        public DateTime date { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
    }
}
