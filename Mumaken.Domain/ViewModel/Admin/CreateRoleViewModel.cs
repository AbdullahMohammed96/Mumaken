using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.ViewModel.Admin
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
