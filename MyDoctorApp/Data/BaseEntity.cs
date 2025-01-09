namespace MyDoctorApp.Data
{
    public abstract class BaseEntity
    {
        public DateTime  InsertedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        
    }
}
