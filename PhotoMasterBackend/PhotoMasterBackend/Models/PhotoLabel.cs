namespace PhotoMasterBackend.Models
{
    public class PhotoLabel : BaseModel
    {
        public int PhotoId { get; set; }
        public int LabelId { get; set; }
        public Photo Photo { get; set; }
        public Label Label { get; set; }
    }
}
