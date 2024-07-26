namespace PostmanTester.Application.Models.Pagination
{
    public class PagingResponse<T> where T : class, new()
    {
        public List<T>? Data { get; set; }
        public Paging Page { get; set; }
    }
}
