using Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RatingService;

public class RatingHostedService : IHostedService,IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private Timer _timer;
    private readonly object _lock = new object(); 
    private readonly List<RatingListItem> _ratingList;
    
    public RatingHostedService(IServiceScopeFactory serviceScopeFactory, List<RatingListItem> ratingList)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _ratingList = ratingList;
    }
    
    public Task StartAsync(CancellationToken cancellationToken) 
    {
        _timer = new Timer(CalculateRatings, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    private async void CalculateRatings(object state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MyBooksDbContext>();

            var newRatingList = await dbContext.Books
                .Where(b => b.Reviews.Count > 0)
                .Select(b => new RatingListItem
                {
                    Id = b.Id,
                    Rating = b.Reviews.Average(r => r.Rating)
                }).ToListAsync();
            
            lock (_lock)
            {
                _ratingList.Clear();
                _ratingList.AddRange(newRatingList);
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

public class RatingListItem
{
    public int Id { get; set; }
    public double Rating { get; set; }
}