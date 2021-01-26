using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoMasterBackend.Models
{
    public class Label : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Color { get; set; }

        public ICollection<PhotoLabel> PhotoLabels { get; set; }
    }
}
