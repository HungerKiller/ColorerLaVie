using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoMasterBackend.Models
{
    public class Photo : BaseModel
    {
        [Required]
        public DateTime Date { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public ICollection<PhotoLabel> PhotoLabels { get; set; }
    }
}
