using Microsoft.AspNetCore.Mvc;
using WebMvcProject.Data;

namespace WebMvcProject.Controllers
{
    public class TeacherController : Controller
    {

        private readonly ProjectDbContext db;
        public TeacherController(ProjectDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Index()
        {

            List<Teacher> teacherList = db.Teachers.ToList();

            return View(teacherList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {
            db.Teachers.Add(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            Teacher teacher = db.Teachers.Find(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);

        }

        [HttpPost]
        public IActionResult Update(Teacher teacher)
        {
            db.Teachers.Update(teacher);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]


        public IActionResult Delete(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            return View(teacher);
        }

        [HttpPost]
        public IActionResult Delete(Teacher teacher)
        {
            db.Teachers.Remove(teacher);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
