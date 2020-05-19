using Microsoft.AspNetCore.Mvc.Rendering;
using RSWEBproekt.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBproekt.ViewModels
{
    public class EnrollStudentsViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<int> SelectedStudents { get; set; }
        public IEnumerable<SelectListItem> StudentList { get; set; }
        public SelectList YearList { get; set; }

        [StringLength(10)]
        public string selectedSemester { get; set; }
        [Display(Name = "Year")]
        public int selectedYear { get; set; }
    }
}
