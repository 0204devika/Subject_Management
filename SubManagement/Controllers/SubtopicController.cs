using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubManagement.Data;
using SubManagement.Models;

namespace SubManagement.Controllers
{
    public class SubtopicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubtopicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubTopics
        public async Task<IActionResult> Index()
        {
            var subTopics = await _context.Subtopics
                                          .Include(st => st.Topic)
                                          .ToListAsync();
            return View(subTopics);
        }


        // GET: SubTopic/Create
        [HttpGet]
        public IActionResult Create()
        {
            var topics = _context.Topics.ToList();
            ViewBag.Topics = new SelectList(topics, "Id", "TopicName");

            return View(new SubTopic());
        }




        // POST: SubTopic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubtopicName,TopicId")] SubTopic subTopic)
        {
            
                ViewBag.Topics = new SelectList(_context.Topics.ToList(), "Id", "TopicName");
            

            _context.Subtopics.Add(subTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: SubTopic/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var subTopic = await _context.Subtopics
                                         .Include(s => s.Topic)
                                         .FirstOrDefaultAsync(s => s.Id == id);
            if (subTopic == null)
            {
                return NotFound();
            }

            ViewBag.Topics = await _context.Topics.ToListAsync();
            return View(subTopic);
        }

        // POST: SubTopic/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubTopic subTopic)
        {
            if (id != subTopic.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(subTopic);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Subtopics.AnyAsync(s => s.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: SubTopic/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subTopic = await _context.Subtopics.FindAsync(id);
            if (subTopic == null)
            {
                return NotFound();
            }

            _context.Subtopics.Remove(subTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
