using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBproekt.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        [Display(Name ="Course")]
        public int CourseID { get; set; }
        [Display(Name = "Student")]
        public int StudentID { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }

        [StringLength(10)]
        public string Semestar { get; set; }

        [Range(5, 10)]
        public int Grade { get; set; }
        public int Year { get; set; }

        [StringLength(255), Display(Name ="Seminal Url"), Url]
        public string SeminalUrl { get; set; }
        [StringLength(255), Display(Name = "Project Url"), Url]
        public string ProjectUrl { get; set; }

        [Display(Name ="Exam Points")]
        public int ExamPoints { get; set; }
        [Display(Name = "Seminal Points")]
        public int SeminalPoints { get; set; }
        [Display(Name = "Project Points")]
        public int ProjectPoints { get; set; }
        [Display(Name = "Additional Points")]
        public int AdditionalPoints { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Finish Date")]
        public DateTime? FinishDate { get; set; }

        [NotMapped]
        [Display(Name = "Seminal Url")]
        public IFormFile SemUrl { get; set; }
    }
}
