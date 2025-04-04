using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly PollDbContext context;

        public PollRepository(PollDbContext _context)
        {
            context = _context;
        }

        public void CreatePoll(Poll poll)
        {
            context.Polls.Add(poll);
            context.SaveChanges();
        }

        public IQueryable<Poll> GetPolls()
        {
            return context.Polls;
        }

        public Poll? GetPollById(int id)
        {
            return context.Polls.FirstOrDefault(p => p.Id == id);
        }

        public void Vote(Poll poll)
        {
            context.Polls.Update(poll);
            context.SaveChanges();
        }
    }
}
