using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSWEBproekt.Data;
using RSWEBproekt.Models;
using RSWEBproekt.ViewModels;

namespace RSWEBproekt.Controllers
{
    public class TeachersController : Controller
    {
        private readonly RSWEBproektContext _context;
        private IWebHostEnvironment WebHostEnvironment { get; }

        public TeachersController(RSWEBproektContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }
        // GET: Teachers
        public async Task<IActionResult> Index(string firstNameString, string lastNameString,
            string degreeString, string academicRankString)
        {
            IQueryable<Teacher> teachers = _context.Teacher.AsQueryable();
            IQueryable<String> degrees = _context.Teacher.OrderBy(m => m.Degree).Select(m => m.Degree).Distinct();
            IQueryable<String> academicRanks = _context.Teacher.OrderBy(m => m.AcademicRank).Select(m => m.AcademicRank).Distinct();
            
            if (!string.IsNullOrEmpty(firstNameString))
            {
                teachers = teachers.Where(t => t.FirstName==firstNameString);
            }
            if (!string.IsNullOrEmpty(lastNameString))
            {
                teachers = teachers.Where(t => t.LastName == lastNameString);
            }
            if (!string.IsNullOrEmpty(degreeString))
            {
                teachers = teachers.Where(t => t.Degree == degreeString);
            }
            if (!string.IsNullOrEmpty(academicRankString))
            {
                teachers = teachers.Where(t => t.AcademicRank == academicRankString);
            }
            var teachersFilterVM = new TeachersFilterViewModel
            {
                Teachers=await teachers.ToListAsync(),
                Degrees=new SelectList(await degrees.ToListAsync()),
                AcademicRanks=new SelectList(await academicRanks.ToListAsync())
            };

            return View(teachersFilterVM);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(p => p.Courses1).Include(p => p.Courses2)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        { 
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile imgUrl, [Bind("TeacherId,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            UploadImage uploadImage = new UploadImage(WebHostEnvironment);
            teacher.ImageUrl = uploadImage.UploadedFile(imgUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.Include(p => p.Courses1).Include(p => p.Courses2)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.TeacherId == id);
        }
    }
}
