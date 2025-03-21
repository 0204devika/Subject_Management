using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubManagement.Data;
using SubManagement.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SubManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Load all saved topics + subtopics to display
            ViewBag.SavedTopics = await _context.Topics
                                        .Include(t => t.Subtopics)
                                        .ToListAsync();

            // Return form model
            var model = new List<Topic>
            {
                new Topic
                {
                    TopicName = "",
                    Subtopics = new List<SubTopic>
                    {
                        new SubTopic { SubtopicName = "" }
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<Topic> topics)
        {
            if (topics == null || !topics.Any())
            {
                ModelState.AddModelError("", "No topics submitted.");
                return View(topics);
            }

            foreach (var topic in topics)
            {
                foreach (var sub in topic.Subtopics)
                {
                    sub.Topic = topic;
                }

                _context.Topics.Add(topic);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Topics and Subtopics saved successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
