using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mumaken.Domain.Entities.UserTables
{
    public class DeviceId
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DeviceId_ { get; set; }
        public string ProjectName { get; set; }
        public string DeviceType { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser user { get; set; }
    }
}
