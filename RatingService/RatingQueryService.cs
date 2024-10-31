﻿namespace RatingService;

public class RatingQueryService
{
    private readonly List<RatingListItem> _ratingList;
    private readonly object _lock = new object();

    public RatingQueryService(List<RatingListItem> popularityList)
    {
        _ratingList = popularityList;
    }
    
    public double GetRating(int id)
    {
        lock (_lock) 
        {
            var rating = _ratingList
                .FirstOrDefault(r => r.Id == id);

            if (rating == null || rating.Rating == 0)
            {
                return Double.NaN;
            }
            
            return rating.Rating;
            
        }
    }
}