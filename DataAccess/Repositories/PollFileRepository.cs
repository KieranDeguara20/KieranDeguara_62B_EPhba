﻿using Domain.Models;
using System.Text.Json;

namespace DataAccess.Repositories
{
    public class PollFileRepository
    {
        private readonly string filePath = "polls.json";

        public void CreatePoll(Poll poll)
        {
            var polls = GetPolls().ToList();
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            polls.Add(poll);
            SavePollsToFile(polls);
        }

        public IQueryable<Poll> GetPolls()
        {
            if (!File.Exists(filePath))
                return new List<Poll>().AsQueryable();

            var json = File.ReadAllText(filePath);
            var polls = JsonSerializer.Deserialize<List<Poll>>(json) ?? new List<Poll>();
            return polls.AsQueryable();
        }

        private void SavePollsToFile(List<Poll> polls)
        {
            var json = JsonSerializer.Serialize(polls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
