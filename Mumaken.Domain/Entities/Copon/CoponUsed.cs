using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.Entities.Copon
{
    public class CoponUsed
    {
        [Key]
        public int Id { get; set; }
        public string CoponId { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
    }
}
