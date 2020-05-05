using Microsoft.AspNetCore.Mvc.Rendering;
using RSWEBproekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBproekt.ViewModels
{
    public class EnrollmentEditViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<int> SelectedStudents { get; set; }
        public IEnumerable<SelectListItem> StudentList { get; set; }
    }
}