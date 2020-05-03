using Microsoft.AspNetCore.Mvc.Rendering;
using RSWEBproekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBproekt.ViewModels
{
    public class CoursesFilterViewModel
    {
        public IList<Course> Courses;
        public SelectList Programmes;
        public SelectList Semestars;
        public string TitleString;
        public string ProgrammeString;
        public int SemestarInt;
    }
}
