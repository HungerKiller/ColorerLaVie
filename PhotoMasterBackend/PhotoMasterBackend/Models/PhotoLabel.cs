namespace PhotoMasterBackend.Models
{
    public class PhotoLabel : BaseModel
    {
        public Photo Photo { get; set; }
        public Label Label { get; set; }
    }
}
