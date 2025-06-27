using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mumaken.Domain.Entities.UserTables
{
    public class NotifyUser
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public DateTime Date { get; set; }
        public int? Type { get; set; }
        public bool? Show { get; set; }
        public int? OrderId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
    }
}
