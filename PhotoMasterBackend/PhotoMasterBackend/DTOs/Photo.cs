using System;
using System.Collections.Generic;

namespace PhotoMasterBackend.DTOs
{
    public class Photo
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Path { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public List<Label> Labels { get; set; }
    }
}
