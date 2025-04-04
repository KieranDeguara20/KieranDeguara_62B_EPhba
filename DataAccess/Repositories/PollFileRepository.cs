using Domain.Interfaces;
using Domain.Models;
using System.Text.Json;

namespace DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
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

        public Poll? GetPollById(int id)
        {
            return GetPolls().FirstOrDefault(p => p.Id == id);
        }

        public void Vote(Poll poll)
        {
            var polls = GetPolls().ToList();
            var index = polls.FindIndex(p => p.Id == poll.Id);
            if (index != -1)
            {
                polls[index] = poll;
                SavePollsToFile(polls);
            }
        }

        private void SavePollsToFile(List<Poll> polls)
        {
            var json = JsonSerializer.Serialize(polls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
