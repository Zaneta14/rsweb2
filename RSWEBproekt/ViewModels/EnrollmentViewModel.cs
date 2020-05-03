using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBproekt.ViewModels
{
    public class EnrollmentViewModel
    {
        [Display(Name = "Seminal Url")]
        public IFormFile SemUrl { get; set; }
    }
}
