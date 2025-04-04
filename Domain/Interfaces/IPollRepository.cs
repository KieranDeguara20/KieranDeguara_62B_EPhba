using Domain.Models;

namespace Domain.Interfaces
{
    public interface IPollRepository
    {
        void CreatePoll(Poll poll);
        IQueryable<Poll> GetPolls();
        Poll? GetPollById(int id);
        void Vote(Poll poll);
    }
}
