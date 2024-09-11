namespace PopularityService
{
    public class PopularityQueryService
    {
        private readonly List<BookPopularity> _popularityList;
        private readonly object _lock = new object(); 

        public PopularityQueryService(List<BookPopularity> popularityList)
        {
            _popularityList = popularityList;
        }

        public List<BookPopularity> GetPopularBooks()
        {
            lock (_lock) // Ensure thread-safe access
            {
                return _popularityList.OrderByDescending(b => b.PopularityScore).ToList();
            }
        }
    }
}