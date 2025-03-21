using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubManagement.Data;
using SubManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubManagement.Controllers
{
    public class TopicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TopicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Show all topics with subtopics
        public async Task<IActionResult> Index()
        {
            var topics = await _context.Topics
                .Include(t => t.Subtopics)
                .ToListAsync();

            return View(topics);
        }

        // ✅ GET: Show form to create topics/subtopics
        [HttpGet]
        public IActionResult Create()
        {
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

            return View(model); // ✅ Pass initialized model
        }

        // ✅ POST: Save topics and subtopics to DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<Topic> topics)
        {
            if (topics == null || !topics.Any())
                return BadRequest("No topics received.");

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
            return RedirectToAction(nameof(Create));
        }

        // ✅ Edit a topic (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _context.Topics
                .Include(t => t.Subtopics)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // ✅ Edit a topic (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Topic topic)
        {
            if (id != topic.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(topic);
            }

            _context.Update(topic);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ✅ Show topic details
        public async Task<IActionResult> Details(int id)
        {
            var topic = await _context.Topics
                .Include(t => t.Subtopics)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // ✅ Confirm delete page
        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _context.Topics
                .Include(t => t.Subtopics)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // ✅ POST: Delete topic and its subtopics
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics
                .Include(t => t.Subtopics)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic != null)
            {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
