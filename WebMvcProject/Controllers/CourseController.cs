using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebMvcProject.Data;
using WebMvcProject.Models;

namespace WebMvcProject.Controllers
{
    //[Authorize(Roles = "admin, user")]
    public class CourseController : Controller
    {
        private readonly ProjectDbContext db;
        public CourseController(ProjectDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult Index(int sayfa = 1, string aranan = "", string fiyatSiralama = "")
        {
            if (aranan == null)
                aranan = "";

            var courseList = db.Courses.Include(x => x.CourseTeacher).Where(x => x.Name.Contains($"{aranan}")).OrderBy(x => x.StartDate).Select(x => new CourseViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Content = x.Content,
                Price = x.Price,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Teacher = x.CourseTeacher != null ? x.CourseTeacher.Name + " " + x.CourseTeacher.Surname : "",
                Students = db.StudentCourses.Where(y => y.CourseId == x.Id).Include(y => y.Student).Select(y => y.Student.Name + " " + y.Student.Surname).ToList()
            }).ToList();

            if (fiyatSiralama == "asc" || string.IsNullOrWhiteSpace(fiyatSiralama))
            {
                courseList = courseList.OrderBy(x => x.Price).ToList();
            }
            else if (fiyatSiralama == "desc")
            {
                courseList = courseList.OrderByDescending(x => x.Price).ToList();
            }

            double kayitSayisi = courseList.Count;
            courseList = courseList.Skip((sayfa - 1) * 5).Take(5).ToList();

            int sayfaSayisi = Convert.ToInt32(Math.Ceiling(kayitSayisi / 5));

            ViewBag.SayfaSayisi = sayfaSayisi;
            ViewBag.OncekiSayfa = sayfa == 1 ? 1 : sayfa - 1;
            ViewBag.SonrakiSayfa = sayfa == sayfaSayisi ? sayfaSayisi : sayfa + 1;

            ViewBag.Aranan = aranan;
            ViewBag.Sayfa = sayfa;
            ViewBag.FiyatSiralama = fiyatSiralama;

            return View(courseList);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Create(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Update(int id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Update(Course course)
        {
            db.Courses.Update(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Course course)
        {
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AssignTeacherToCourse(int? id)
        {
            var courses = db.Courses.FirstOrDefault(sc => sc.Id == id);
            if (courses == null)
                return NotFound();

            ViewBag.Teachers = db.Teachers.ToList();//Öğretmenler listesi
            ViewBag.Courses = db.Courses.Where(x => x.Id == id).ToList(); //Kurslar listesi

            CourseTeacherViewModel courseTeacher = new CourseTeacherViewModel() { CourseId = id.Value, TeacherId = courses.CourseTeacherId };

            return View(courseTeacher);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AssignTeacherToCourse(CourseTeacherViewModel courseTeacher)
        {
            ViewBag.Teachers = db.Teachers.ToList();
            ViewBag.Courses = db.Courses.ToList();

            if (ModelState.IsValid)
            {
                // Veritabanında böyle bir atama kaydının olup olmadığını kontrol et
                bool isAlreadyAssigned = db.Courses.Any(sc => sc.Id == courseTeacher.CourseId && sc.CourseTeacherId == courseTeacher.TeacherId);
                if (!isAlreadyAssigned)
                {
                    Course course = db.Courses.FirstOrDefault(x => x.Id == courseTeacher.CourseId);
                    course.CourseTeacherId = courseTeacher.TeacherId;

                    db.Courses.Update(course);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Bu eğitmen zaten bu kursa atanmış.");
                    return View(courseTeacher);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public IActionResult AssignStudentToCourse(int? id)
        {

            if (User.Identity.IsAuthenticated)
            {
                string Username = User.FindFirst("Username").Value;
            }

            var courses = db.Courses.FirstOrDefault(sc => sc.Id == id);
            if (courses == null)
                return NotFound();

            ViewBag.Students = db.Students.Select(x => new StudentViewModel() { Id = x.Id, Name = x.Name, Surname = x.Surname, IsActive = !db.StudentCourses.Any(y => y.StudentId == x.Id && y.CourseId == id) }).ToList();//Öğrenciler listesi
            ViewBag.Courses = db.Courses.Where(x => x.Id == id).ToList(); //Kurslar listesi

            ViewBag.CourseStudents = db.StudentCourses.Include(x => x.Student).Where(x => x.CourseId == id).ToList(); //Kurs Öğrenci Listesi

            CourseStudentViewModel courseStudent = new CourseStudentViewModel() { CourseId = id.Value };

            return View(courseStudent);
        }

        [HttpPost]
        [Authorize(Roles = "admin,user")]
        public IActionResult AssignStudentToCourse(Models.CourseStudentViewModel courseStudent)
        {
            var courses = db.Courses.FirstOrDefault(sc => sc.Id == courseStudent.CourseId);
            if (courses == null)
                return NotFound();

            ViewBag.Students = db.Students.Select(x => new StudentViewModel() { Id = x.Id, Name = x.Name, Surname = x.Surname, IsActive = !db.StudentCourses.Any(y => y.StudentId == x.Id && y.CourseId == courseStudent.CourseId) }).ToList();//Öğrenciler listesi
            ViewBag.Courses = db.Courses.Where(x => x.Id == courseStudent.CourseId).ToList(); //Kurslar listesi
            ViewBag.CourseStudents = db.StudentCourses.Include(x => x.Student).Where(x => x.CourseId == courseStudent.CourseId).ToList(); //Kurs Öğrenci Listesi

            if (ModelState.IsValid)
            {
                // Veritabanında böyle bir atama kaydının olup olmadığını kontrol et
                bool isAlreadyAssigned = db.StudentCourses.Any(sc => sc.StudentId == courseStudent.StudentId && sc.CourseId == courseStudent.CourseId);

                if (!isAlreadyAssigned)
                {
                    Data.StudentCourse newStudentCourse = new Data.StudentCourse
                    {
                        StudentId = courseStudent.StudentId.Value,
                        CourseId = courseStudent.CourseId.Value
                    };

                    db.StudentCourses.Add(newStudentCourse);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Bu öğrenci zaten bu kursa atanmış.");
                    return View(courseStudent);
                }

                return RedirectToAction("Index");
            }

            return View(courseStudent);
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult RemoveStudentToCourse(int id)
        {
            var studentCourse = db.StudentCourses.Include(x => x.Student).Include(x => x.Course).FirstOrDefault(x => x.StudentCourseId == id);
            if (studentCourse == null)
                return NotFound();

            db.StudentCourses.Remove(studentCourse);
            db.SaveChanges();

            return RedirectToAction("AssignStudentToCourse", new { id = studentCourse.CourseId });
        }
    }
}