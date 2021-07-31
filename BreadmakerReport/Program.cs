using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers.Include(r1=> r1.Reviews).AsEnumerable().Select(r2 => new
            {
                TotalReviews = r2.Reviews.Count,
                AverageReviews = Math.Round(r2.Reviews.Average(r1Object => r1Object.stars),2),
                AdjustReview = Math.Round(ratingAdjustmentService.Adjust(r2.Reviews.Average(r1Object => r1Object.stars), r2.Reviews.Count()),2),
                r2.title
            })
                .OrderByDescending(r2 => r2.AdjustReview)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var i = 0; i < 3; i++)
            {
                var j = BMList[i];
                // TODO: add output
                Console.WriteLine( "[{0}] {1} {2} {3} {4}", i+1, j.TotalReviews, j.AverageReviews, j.AdjustReview, j.title);
            }
        }
    }
}
