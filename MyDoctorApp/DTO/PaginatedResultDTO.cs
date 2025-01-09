namespace MyDoctorApp.DTO
{
    public class PaginatedResultDTO<T>
    {
        public List<T> Data { get; set; } = null!;
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
