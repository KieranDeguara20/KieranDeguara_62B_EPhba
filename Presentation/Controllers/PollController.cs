using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices] IPollRepository pollRepository)
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
        public IActionResult CreatePoll(Poll poll, [FromServices] IPollRepository pollRepository)
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
        public IActionResult Details(int id, [FromServices] IPollRepository pollRepository)
        {
            var poll = pollRepository.GetPollById(id);
            if (poll == null)
                return NotFound();
            return View(poll);
        }

        [HttpPost]
        public IActionResult Vote(int id, string selectedOption, [FromServices] IPollRepository pollRepository, [FromServices] UserManager<IdentityUser> userManager)
        {
            var poll = pollRepository.GetPollById(id);
            if (poll == null)
                return NotFound();

            var userId = userManager.GetUserId(User);
            if (poll.Voters.Contains(userId))
            {
                return RedirectToAction("Details", new { id, message = "You have already voted in this poll." });
            }

            if (!string.IsNullOrEmpty(selectedOption))
            {
                switch (selectedOption)
                {
                    case "Option1": poll.Option1VotesCount++; break;
                    case "Option2": poll.Option2VotesCount++; break;
                    case "Option3": poll.Option3VotesCount++; break;
                }
                poll.Voters.Add(userId); 
                pollRepository.Vote(poll);
            }
            return RedirectToAction("Details", new { id });
        }
    }
}
