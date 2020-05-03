using Microsoft.AspNetCore.Mvc.Rendering;
using RSWEBproekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBproekt.ViewModels
{
    public class TeachersFilterViewModel
    {
        public IList<Teacher> Teachers;
        public SelectList Degrees;
        public SelectList AcademicRanks;
        public string FirstNameString;
        public string LastNameString;
        public string DegreeString;
        public string AcademicRankString;
    }
}
