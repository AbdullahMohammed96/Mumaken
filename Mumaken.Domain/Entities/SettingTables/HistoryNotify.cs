using Mumaken.Domain.Entities.UserTables;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mumaken.Domain.Entities.SettingTables
{
    // Table History Text
    public class HistoryNotify
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int CountNotify { get; set; }
    }
}
