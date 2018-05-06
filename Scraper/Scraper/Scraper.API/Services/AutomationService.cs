﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scraper.Core.Data;
using Scraper.Core.Model;
using Scraper.Core.Scraper.DanskeSpil;

namespace Scraper.API.Services
{
    public class AutomationService
    {
        private readonly DanskeSpilScraper _scraper = new DanskeSpilScraper();
        private readonly IServiceScopeFactory _scopeFactory;

        public AutomationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ScrapeUpcomingMatches()
        {
            var matches = new List<Match>(_scraper.GetUpcomingMatches());

            var validMatches = matches
                .Select(m => m)
                .Where(m => WithinValidDate(m.MatchDate))
                .ToList();

            var subMatches = new List<Match>();

            Parallel.ForEach(validMatches, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (match) =>
            {
                var sms = _scraper.GetSubMatches(match.EventId);
                foreach (var sm in sms)
                {
                    sm.ParentId = match.MatchId;
                    sm.EventId = match.EventId;
                    sm.RoundId = sm.RoundId == -1 ? match.RoundId : -1;
                }
                subMatches.AddRange(sms);
            });

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    using (var db = scope.ServiceProvider.GetRequiredService<DanskeSpilContext>())
                    {
                        db.Matches.AddRange(validMatches);
                        db.Matches.AddRange(subMatches);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        /// Valid matches is found between Saturday 12.00 and Monday 23.59
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static bool WithinValidDate(DateTime date)
        {

            var nextSaturday = GetNextWeekday(DateTime.Now.Date, DayOfWeek.Saturday).AddHours(12);
            var nextMonday = GetNextWeekday(DateTime.Now.Date, DayOfWeek.Monday).AddHours(23).AddMinutes(59);

            return date >= nextSaturday && date <= nextMonday;
        }

        private static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
    }
}