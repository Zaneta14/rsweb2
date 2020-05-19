using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using RSWEBproekt.Data;
using RSWEBproekt.Models;
using RSWEBproekt.ViewModels;
using Microsoft.AspNetCore.Http;

namespace RSWEBproekt.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly RSWEBproektContext _context;
        private IWebHostEnvironment WebHostEnvironment { get; }


        public EnrollmentsController(RSWEBproektContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> GetStudentsByCourse(int id, int? yearInt = 0)
        {
            var course = _context.Course.Where(l => l.CourseID == id).FirstOrDefault();
            ViewData["courseName"] = course.Title;
            TempData["selectedCourse"] = id.ToString();
            var enrollments = _context.Enrollment.Where(s => s.CourseID == id);
            string tch = TempData["selectedTeacher"].ToString();
            TempData.Keep();
            ViewData["tch"] = tch;
            enrollments = enrollments.Include(c => c.Student);
            if (yearInt==0)
            {
                string yearStr= DateTime.Now.Year.ToString();
                yearInt = Int32.Parse(yearStr);
            }
            enrollments = enrollments.Where(s => s.Year == yearInt);
            ViewData["currentYear"] = yearInt;
            return View(enrollments);
        }

        public async Task<IActionResult> EditByProfessor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string s = null;
            if (TempData["selectedCourse"]!=null)
                s=TempData["selectedCourse"].ToString();
            TempData.Keep();
            ViewData["selectedCrs"] = s;
            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }

        [HttpPost, ActionName("editByProfessor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditByProfessorPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int crsId = 1;
            string crs = null;
            if (TempData["selectedCourse"]!=null)
            {
              crs = TempData["selectedCourse"].ToString();
              crsId = Int32.Parse(crs);
            }
           
            var enrollmentToUpdate = await _context.Enrollment.FirstOrDefaultAsync(s => s.EnrollmentID == id);
            await TryUpdateModelAsync<Enrollment>(
                 enrollmentToUpdate,
                 "", s => s.ExamPoints, s => s.SeminalPoints, s => s.ProjectPoints, s => s.AdditionalPoints,
                 s => s.Grade, s => s.FinishDate);
            
                try
                {
                    await _context.SaveChangesAsync();
                return RedirectToAction("getStudentsByCourse", "Enrollments", new { id = crsId });
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex vari  able name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            return View(enrollmentToUpdate);
        }

        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "seminals");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public async Task<IActionResult> EditByStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string c = null;
            if (TempData["selectedStudent"]!=null)
                c = TempData["selectedStudent"].ToString();
            TempData.Keep();
            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }

        [HttpPost, ActionName("editByStudent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditByStudentPost(int? id, IFormFile semUrl)
        { 
            if (id == null)
            {
                return NotFound();
            }

            int stId = 1;
            string st = null;
            if (TempData["selectedStudent"] != null)
            {
                st = TempData["selectedStudent"].ToString();
                stId = Int32.Parse(st);
            }

            var enrollmentToUpdate = await _context.Enrollment.FirstOrDefaultAsync(s => s.EnrollmentID == id);
            enrollmentToUpdate.SeminalUrl = UploadedFile(semUrl);
            await TryUpdateModelAsync<Enrollment>(
                enrollmentToUpdate,
                "", s => s.ProjectUrl);
        
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("getCoursesByStudent", "Enrollments", new { id = stId } );
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            //}
            return View(enrollmentToUpdate);
        }

        
        public async Task<IActionResult> GetCoursesByStudent(int id)
        {
            TempData["selectedStudent"] = id.ToString();
            var student = _context.Student.Where(s => s.Id == id).First();
            ViewData["studentName"]=student.FullName;
            var enrollments = _context.Enrollment.Where(s => s.StudentID == id);
            enrollments = enrollments.Include(c => c.Course);
            return View(enrollments);
        }
        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var rSWEBproektContext = _context.Enrollment.Include(e => e.Course).Include(e => e.Student);
            return View(await rSWEBproektContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string s = null;
            if (TempData["selectedCourse"] != null)
                s = TempData["selectedCourse"].ToString();
            TempData.Keep();
            ViewData["selectedCrs"] = s;
            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        public async Task<IActionResult> EnrollStudents(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Course.Where(m => m.CourseID == id)
               .Include(m => m.Students).First();
            int nowYear = System.DateTime.Now.Year;
            int[] years = Enumerable.Range(nowYear - 10, 11).Reverse().ToArray();
            var EnrollStudentsVM = new EnrollStudentsViewModel
            {
                Course = course,
                StudentList = new MultiSelectList(_context.Student, "Id", "FullName"),
                YearList = new SelectList(years.ToList())
            };
            return View(EnrollStudentsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollStudents(int id, EnrollStudentsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                foreach(var student in viewModel.SelectedStudents)
                {
                    Enrollment enrollment = await _context.Enrollment
                        .FirstOrDefaultAsync(m => m.CourseID == id && m.StudentID == student 
                        && m.Year == viewModel.selectedYear && m.Semestar == viewModel.selectedSemester);
                    if (enrollment==null)
                    {
                        enrollment = new Enrollment
                        {
                            CourseID = id,
                            StudentID = student,
                            Year = viewModel.selectedYear,
                            Semestar = viewModel.selectedSemester
                        };
                        _context.Add(enrollment);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CoursesController.Index));
            }
            return View(viewModel);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "Title");
            ViewData["StudentID"] = new SelectList(_context.Student, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,CourseID,StudentID,Semestar,Grade,Year,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var enrollmentToUpdate = await _context.Enrollment.FirstOrDefaultAsync(s => s.EnrollmentID == id);
            await TryUpdateModelAsync<Enrollment>(
                enrollmentToUpdate,
                "",
                s => s.FinishDate);
            //{
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            //}
            return View(enrollmentToUpdate);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.EnrollmentID == id);
        }
    }
}
