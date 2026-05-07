using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementCore.Models.ViewModels
{
    public class ClientVM
    {
        public int ClientId { get; set; }
        [Display(Name = "Client Name"), Required]
        public string ClientName { get; set; } = default!;
        [Required, Display(Name = "Date Of Birth"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string? Image { get; set; } = default!;
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "Marital Status")]
        public bool MaritalStatus { get; set; }
        public List<int> EventList { get; set; }=new List<int>();
    }
}
