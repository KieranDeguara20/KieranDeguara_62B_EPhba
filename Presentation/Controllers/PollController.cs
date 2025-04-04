using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices] PollRepository pollRepository)
        {
            var polls = pollRepository.GetPolls().OrderByDescending(p => p.DateCreated);
            return View(polls);
        }

        [HttpGet]
        public IActionResult CreatePoll()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePoll(Poll poll, [FromServices] PollRepository pollRepository)
        {
            if (ModelState.IsValid)
            {
                poll.DateCreated = DateTime.Now;
                poll.Option1VotesCount = 0;
                poll.Option2VotesCount = 0;
                poll.Option3VotesCount = 0;

                pollRepository.CreatePoll(poll);
                return View();
            }
            return View(poll);
        }

        [HttpGet]
        public IActionResult Details(int id, [FromServices] PollRepository pollRepository)
        {
            var poll = pollRepository.GetPollById(id);
            if (poll == null) 
                return NotFound();
            return View(poll);
        }
    }
}
