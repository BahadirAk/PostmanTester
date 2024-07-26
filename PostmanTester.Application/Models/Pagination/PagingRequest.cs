namespace PostmanTester.Application.Models.Pagination
{
    public class PagingRequest
    {
        private int page;
        private int size;
        public int Page { get { return page; } set { page = value <= 0 ? 1 : value; } }
        public int Size { get { return size; } set { size = value <= 0 ? 10 : value; } }
    }
}
