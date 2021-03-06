﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSWEBproekt.Data;
using RSWEBproekt.Models;

namespace RSWEBproekt.Controllers
{
    public class StudentsController : Controller
    {
        private readonly RSWEBproektContext _context;
        private IWebHostEnvironment WebHostEnvironment { get; }

        public StudentsController(RSWEBproektContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentIdString, string firstNameString, string lastNameString)
        {
             ViewData["CurrentSI"] = studentIdString;
               ViewData["CurrentFN"] = firstNameString;
               ViewData["CurrentLN"] = lastNameString;
               var students = _context.Student.AsQueryable();
               //var students = _context.Student.Include(m => m.Courses)
                  // .ThenInclude(m => m.Course).AsNoTracking();
               if (!string.IsNullOrEmpty(studentIdString))
                   students = students.Where(m => m.StudentId == studentIdString);
               if (!string.IsNullOrEmpty(firstNameString))
                   students = students.Where(m => m.FirstName == firstNameString);
               if (!string.IsNullOrEmpty(lastNameString))
                   students = students.Where(m => m.LastName == lastNameString);
            
           students = students.Include(m => m.Courses).ThenInclude(m => m.Course);            
            return View(await students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(m => m.Courses).ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemestar,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile imgUrl1, [Bind("Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemestar,EducationLevel")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }
            UploadImage uploadImage = new UploadImage(WebHostEnvironment);
            student.ImageUrl = uploadImage.UploadedFile(imgUrl1);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(m => m.Courses).ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
