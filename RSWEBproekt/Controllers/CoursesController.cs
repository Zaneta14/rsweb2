using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSWEBproekt.Data;
using RSWEBproekt.Models;
using RSWEBproekt.ViewModels;

namespace RSWEBproekt.Controllers
{
    public class CoursesController : Controller
    {
        private readonly RSWEBproektContext _context;

        public CoursesController(RSWEBproektContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetCoursesByTeacher(int id)
        {
            var teacher = _context.Teacher.Where(m => m.TeacherId == id).FirstOrDefault();
            ViewData["teacherFullName"] = teacher.FullName;
            TempData["selectedTeacher"] = id.ToString();
            var courses = _context.Course.Where(s => s.FirstTeacherId == id || s.SecondTeacherId == id);
            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher);
            return View(courses);
        }

        // GET: Courses
        public async Task<IActionResult> Index(string titleString, string programmeString, int semestarInt=0)
        {
            TempData["test"] = "lala";
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<String> programmes = _context.Course.OrderBy(m => m.Programme)
                .Select(m => m.Programme).Distinct();
            IQueryable<int> semesters = _context.Course.OrderBy(m => m.Semestar)
                .Select(m => m.Semestar).Distinct();
            if (!string.IsNullOrEmpty(titleString))
            {
                courses = courses.Where(s => s.Title.Contains(titleString));    
            }
            if (!string.IsNullOrEmpty(programmeString))
            {
                courses = courses.Where(s => s.Programme == programmeString);
            }
            if (semestarInt!=0)
            {
                courses = courses.Where(s => s.Semestar == semestarInt);
            }
            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher);
            var coursesFilterVM = new CoursesFilterViewModel
            {
                Courses = await courses.ToListAsync(),
                Programmes = new SelectList(await programmes.ToListAsync()),
                Semestars = new SelectList(await semesters.ToListAsync())
            };
            return View(coursesFilterVM);

            
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            String s = null;
            if (TempData["test"]!=null)
                s = TempData["test"].ToString();
            TempData.Keep();
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,Semestar,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {           
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            String s = null;
            if (TempData["test"]!=null)
                s = TempData["test"].ToString();
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Course.Where(m=> m.CourseID==id)
                .Include(m=>m.Students).First();
            if (course == null)
            {
                return NotFound();
            }
            
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Title,Credits,Semestar,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            String s = null;
            if (TempData["test"]!=null)
                s = TempData["test"].ToString();
            if (id != course.CourseID)
            {
                return NotFound();
            }

           if (ModelState.IsValid)
           {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }
    }
}
