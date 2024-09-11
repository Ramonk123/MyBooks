using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBooks.Libraries.Data;

namespace PopularityService
{
    public class PopularityHostedService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly List<BookPopularity> _popularityList;
        private Timer _timer;
        private readonly object _lock = new object(); 

        public PopularityHostedService(IServiceScopeFactory serviceScopeFactory, List<BookPopularity> popularityList)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _popularityList = popularityList;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CalculatePopularity, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void CalculatePopularity(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyBooksDbContext>();

                var books = await dbContext.LibraryBooks
                    .GroupBy(lb => lb.BookId)
                    .Select(b => new
                    {
                        BookId = b.Key,
                        PopularityScore = b.Count(),
                    })
                    .OrderByDescending(b => b.PopularityScore)
                    .Take(20)
                    .ToListAsync();

                var bookIds = books.Select(b => b.BookId).ToList();

                var dbBooks = await dbContext.Books
                    .Where(b => bookIds.Contains(b.Id))
                    .Include(b => b.Author)
                    .ToListAsync();

                var formattedList = dbBooks.Select(b => new BookPopularity
                {
                    BookId = b.PublicId,
                    Title = b.Title,
                    Author = b.Author.Name,
                    AuthorId = b.Author.PublicId,
                    PopularityScore = books.First(book => book.BookId == b.Id).PopularityScore,
                    ThumbnailUrl = b.ThumbnailURL
                }).ToList();

                lock (_lock)
                {
                    _popularityList.Clear();
                    _popularityList.AddRange(formattedList);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}


public class BookPopularity
{
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Guid AuthorId { get; set; }
    public int PopularityScore { get; set; }
    public string ThumbnailUrl { get; set; }
}
