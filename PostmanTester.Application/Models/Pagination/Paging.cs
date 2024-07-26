namespace PostmanTester.Application.Models.Pagination
{
    public class Paging
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }

        public bool HasNext => Page != TotalPage;
        public bool HasPrevious => Page != 1;
    }
}
