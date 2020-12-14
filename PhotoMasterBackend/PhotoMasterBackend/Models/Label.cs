using System.ComponentModel.DataAnnotations;

namespace PhotoMasterBackend.Models
{
    public class Label : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
