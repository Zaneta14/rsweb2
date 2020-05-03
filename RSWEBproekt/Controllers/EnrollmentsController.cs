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

        public async Task<IActionResult> editByProfessor(int? id)
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

        [HttpPost, ActionName("editByProfessor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editByProfessorPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var enrollmentToUpdate = await _context.Enrollment.FirstOrDefaultAsync(s => s.EnrollmentID == id);
            if (await TryUpdateModelAsync<Enrollment>(
                enrollmentToUpdate,
                "", s => s.ExamPoints, s => s.SeminalPoints, s => s.ProjectPoints, s => s.AdditionalPoints,
                s => s.Grade, s => s.FinishDate))
            {
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
            }
            return View(enrollmentToUpdate);
        }

        private string UploadedFile(Enrollment model)
        {
            string uniqueFileName = null;

            if (model.SemUrl != null)
            {
                string uploadsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.SemUrl.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.SemUrl.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public async Task<IActionResult> editByStudent(int? id)
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

        [HttpPost, ActionName("editByStudent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editByStudentPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollmentToUpdate = await _context.Enrollment.FirstOrDefaultAsync(s => s.EnrollmentID == id);
            //enrollmentToUpdate.SeminalUrl = UploadedFile(enrollmentToUpdate); //raboti projecturl
            enrollmentToUpdate.SeminalUrl = "test";//ne raboti
            if (await TryUpdateModelAsync<Enrollment>(
                enrollmentToUpdate,
                "", s => s.ProjectUrl, s=>s.SeminalUrl))
            {
                try
                {
                   // enrollmentToUpdate.SeminalUrl = "test"; //raboti sose s=>s.seminalurl
                    //enrollmentToUpdate.SeminalUrl = UploadedFile(enrollmentToUpdate); ne raboti
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
            }
            return View(enrollmentToUpdate);
        }

        public async Task<IActionResult> getStudentsByCourse(int id, int? yearInt=0)
        {
            var enrollments = _context.Enrollment.Where(s => s.CourseID == id);
            enrollments = enrollments.Include(c => c.Student);
            if (yearInt != 0)
            {
                enrollments = enrollments.Where(s => s.Year == yearInt);
            }
            return View(enrollments);
        }

        public async Task<IActionResult> getCoursesByStudent(int studentId)
        {
            var enrollments = _context.Enrollment.Where(s => s.StudentID == studentId);
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
            if (course == null)
            {
                return NotFound();
            }
            var EnrollmentEditVM = new EnrollmentEditViewModel
            {
                Course = course,
                SelectedStudents = course.Students.Select(m => m.StudentID),
                StudentList = new MultiSelectList(_context.Student, "Id", "FullName")
            };
            return View(EnrollmentEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollStudents(int id, int year, string semester, EnrollmentEditViewModel viewModel)
        {
            /*if (id != viewModel.Course.CourseID)
            {
                return NotFound();
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<int> listStudents = viewModel.SelectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !listStudents.Contains(s.StudentID) && s.CourseID == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);
                    IEnumerable<int> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentID) && s.CourseID == id).Select(s => s.StudentID);
                    IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s));
                    
                    foreach (int studentId in newStudents)
                    _context.Enrollment.Add(new Enrollment { StudentID = studentId, CourseID = id,
                    Semestar=semester, Year=year });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                /*if (!CourseExists(viewModel.Course.CourseID))
                {
                    return NotFound();
                }
                else
                {*/
                    throw;
                //}
                }
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

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            if (await TryUpdateModelAsync<Enrollment>(
                enrollmentToUpdate,
                "",
                s => s.FinishDate))
            {
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
            }
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
