using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RSWEBproekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBproekt.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RSWEBproektContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RSWEBproektContext>>()))
            {
                // Look for any movies.
                if (context.Student.Any() || context.Teacher.Any() || context.Course.Any())
                {
                    return;   // DB has been seeded
                }

                context.Student.AddRange(

                new Student
                {
                    FirstName = "Carson",
                    LastName = "Alexander",
                    StudentId = "9/2010",
                    AcquiredCredits = 200,
                    CurrentSemestar = 6,
                    EducationLevel = "Junior",
                    EnrollmentDate = DateTime.Parse("2010-09-01")
                },
                new Student
                {
                    FirstName = "Meredith",
                    LastName = "Alonso",
                    StudentId = "100/2012",
                    AcquiredCredits = 150,
                    CurrentSemestar = 5,
                    EducationLevel = "Sophomore",
                    EnrollmentDate = DateTime.Parse("2012-09-01")
                },
                new Student
                {
                    FirstName = "Arturo",
                    LastName = "Anand",
                    StudentId = "33/2013",
                    AcquiredCredits = 30,
                    CurrentSemestar = 1,
                    EducationLevel = "Freshman",
                    EnrollmentDate = DateTime.Parse("2013-09-01")
                },
                new Student
                {
                    FirstName = "Gytis",
                    LastName = "Barzdukas",
                    StudentId = "432/2012",
                    AcquiredCredits = 232,
                    CurrentSemestar = 8,
                    EducationLevel = "Senior",
                    EnrollmentDate = DateTime.Parse("2012-09-01")
                },
                new Student
                {
                    FirstName = "Yan",
                    LastName = "Li",
                    StudentId = "120/2012",
                    AcquiredCredits = 100,
                    CurrentSemestar = 4,
                    EducationLevel = "Sophomore",
                    EnrollmentDate = DateTime.Parse("2012-09-01")
                },
                new Student
                {
                    FirstName = "Peggy",
                    LastName = "Justice",
                    StudentId = "90/2011",
                    AcquiredCredits = 60,
                    CurrentSemestar = 2,
                    EducationLevel = "Freshman",
                    EnrollmentDate = DateTime.Parse("2011-09-01")
                },
                new Student
                {
                    FirstName = "Laura",
                    LastName = "Norman",
                    StudentId = "7/2013",
                    AcquiredCredits = 240,
                    CurrentSemestar = 8,
                    EducationLevel = "Senior",
                    EnrollmentDate = DateTime.Parse("2013-09-01")
                },
                new Student
                {
                    FirstName = "Nino",
                    LastName = "Olivetto",
                    StudentId = "300/2005",
                    AcquiredCredits = 132,
                    CurrentSemestar = 5,
                    EducationLevel = "Junior",
                    EnrollmentDate = DateTime.Parse("2005-09-01")
                }
            );
                context.SaveChanges();

                context.Teacher.AddRange(
                     new Teacher
                     {
                         FirstName = "Kim",
                         LastName = "Abercrombie",
                         HireDate = DateTime.Parse("1995-03-11"),
                         Degree = "Master of Science",
                         AcademicRank = "Associate Professor",
                         OfficeNumber = "121 A"
                     },
                new Teacher
                {
                    FirstName = "Fadi",
                    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06"),
                    Degree = "Master of Science",
                    AcademicRank = "Assistant Professor",
                    OfficeNumber = "121 B"
                },
                new Teacher
                {
                    FirstName = "Roger",
                    LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01"),
                    Degree = "Doctor of Science",
                    AcademicRank = "Associate Professor",
                    OfficeNumber = "312"
                },
                new Teacher
                {
                    FirstName = "Candace",
                    LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15"),
                    Degree = "Doctor of Science",
                    AcademicRank = "Professor",
                    OfficeNumber = "111 C"
                },
                new Teacher
                {
                    FirstName = "Roger",
                    LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12"),
                    Degree = "Doctor of Science",
                    AcademicRank = "Professor",
                    OfficeNumber = "200"
                },
                new Teacher
                {
                    FirstName = "Justin",
                    LastName = "Schraner",
                    HireDate = DateTime.Parse("2007-05-12"),
                    Degree = "Doctor of Science",
                    AcademicRank = "Professor",
                    OfficeNumber = "100 B"
                },
                new Teacher
                {
                    FirstName = "Joanna",
                    LastName = "Frank",
                    HireDate = DateTime.Parse("2010-03-04"),
                    Degree = "Master of Science",
                    AcademicRank = "Assistant Professor",
                    OfficeNumber = "212 C"
                }
               );
                context.SaveChanges();

                context.Course.AddRange(
                    new Course
                    {
                        Title = "Complex analysis",
                        Credits = 3,
                        Semestar = 4,
                        Programme = "Programme",
                        EducationLevel = "Sophomore",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Roger" && d.LastName == "Zheng").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Joanna" && d.LastName == "Frank").TeacherId
                    },
                    new Course
                    {
                        Title = "Network programming",
                        Credits = 5,
                        Semestar = 5,
                        Programme = "Programme",
                        EducationLevel = "Junior",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Justin" && d.LastName == "Schraner").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Joanna" && d.LastName == "Frank").TeacherId
                    },
                    new Course
                    {
                        Title = "Telecommunications networks",
                        Credits = 6,
                        Semestar = 6,
                        Programme = "Programme",
                        EducationLevel = "Junior",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Kim" && d.LastName == "Abercrombie").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Fadi" && d.LastName == "Fakhouri").TeacherId
                    },
                    new Course
                    {
                        Title = "Mathematics 1",
                        Credits = 7,
                        Semestar = 1,
                        Programme = "Programme",
                        EducationLevel = "Freshman",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Kim" && d.LastName == "Abercrombie").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Roger" && d.LastName == "Zheng").TeacherId
                    },
                    new Course
                    {
                        Title = "Wireless networks",
                        Credits = 6,
                        Semestar = 7,
                        Programme = "Programme",
                        EducationLevel = "Senior",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Roger" && d.LastName == "Harui").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Candace" && d.LastName == "Kapoor").TeacherId
                    },
                    new Course
                    {
                        Title = "Basics of electronics",
                        Credits = 4,
                        Semestar = 3,
                        Programme = "Programme",
                        EducationLevel = "Sophomore",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Roger" && d.LastName == "Harui").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Roger" && d.LastName == "Zheng").TeacherId
                    },
                    new Course
                    {
                        Title = "Electric power systems",
                        Credits = 6,
                        Semestar = 5,
                        Programme = "Programme",
                        EducationLevel = "Junior",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Candace" && d.LastName == "Kapoor").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Fadi" && d.LastName == "Fakhouri").TeacherId
                    },
                    new Course
                    {
                        Title = "Photovoltaic systems",
                        Credits = 3,
                        Semestar = 8,
                        Programme = "Programme",
                        EducationLevel = "Senior",
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Justin" && d.LastName == "Schraner").TeacherId,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Kim" && d.LastName == "Abercrombie").TeacherId
                    }
                    );
                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment
                    {
                        StudentID = 1,
                        CourseID = 1,
                        Semestar = "Summer, 2015/2016",
                        Grade = 10,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 90,
                        SeminalPoints = 95,
                        ProjectPoints = 100,
                        AdditionalPoints = 5,
                        FinishDate = DateTime.Parse("2016-05-15")
                    },
                    new Enrollment
                    {
                        StudentID = 1,
                        CourseID = 2,
                        Semestar = "Summer, 2015/2016",
                        Grade = 8,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 70,
                        SeminalPoints = 90,
                        ProjectPoints = 100,
                        AdditionalPoints = 0,
                        FinishDate = DateTime.Parse("2016-05-20")
                    },
                    new Enrollment
                    {
                        StudentID = 2,
                        CourseID = 3,
                        Semestar = "Winter, 2015/2016",
                        Grade = 10,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 99,
                        SeminalPoints = 98,
                        ProjectPoints = 100,
                        AdditionalPoints = 0,
                        FinishDate = DateTime.Parse("2015-12-30")
                    },
                    new Enrollment
                    {
                        StudentID = 2,
                        CourseID = 4,
                        Semestar = "Summer, 2012/2013",
                        Grade = 6,
                        Year = 2013,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 58,
                        SeminalPoints = 70,
                        ProjectPoints = 35,
                        AdditionalPoints = 5,
                        FinishDate = DateTime.Parse("2013-11-15")
                    },
                    new Enrollment
                    {
                        StudentID = 3,
                        CourseID = 5,
                        Semestar = "Winter, 2010/2011",
                        Grade = 9,
                        Year = 2010,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 90,
                        SeminalPoints = 95,
                        ProjectPoints = 65,
                        AdditionalPoints = 15,
                        FinishDate = DateTime.Parse("2010-01-10")
                    },
                    new Enrollment
                    {
                        StudentID = 3,
                        CourseID = 6,
                        Semestar = "Summer, 2015/2016",
                        Grade = 10,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 88,
                        SeminalPoints = 100,
                        ProjectPoints = 100,
                        AdditionalPoints = 0,
                        FinishDate = DateTime.Parse("2016-05-15")
                    },
                    new Enrollment
                    {
                        StudentID = 4,
                        CourseID = 7,
                        Semestar = "Winter, 2018/2019",
                        Grade = 7,
                        Year = 2018,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 78,
                        SeminalPoints = 60,
                        ProjectPoints = 67,
                        AdditionalPoints = 8,
                        FinishDate = DateTime.Parse("2018-12-10")
                    },
                    new Enrollment
                    {
                        StudentID = 4,
                        CourseID = 8,
                        Semestar = "Summer, 2015/2016",
                        Grade = 5,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 20,
                        SeminalPoints = 55,
                        ProjectPoints = 30,
                        AdditionalPoints = 10,
                        FinishDate = DateTime.Parse("2016-05-01")
                    },
                    new Enrollment
                    {
                        StudentID = 5,
                        CourseID = 1,
                        Semestar = "Summer, 2015/2016",
                        Grade = 5,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 20,
                        SeminalPoints = 55,
                        ProjectPoints = 30,
                        AdditionalPoints = 10,
                        FinishDate = DateTime.Parse("2016-05-01")
                    },
                    new Enrollment
                    {
                        StudentID = 5,
                        CourseID = 2,
                        Semestar = "Summer, 2010/2011",
                        Grade = 5,
                        Year = 2011,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 44,
                        SeminalPoints = 90,
                        ProjectPoints = 60,
                        AdditionalPoints = 0,
                        FinishDate = DateTime.Parse("2011-03-22")
                    },
                    new Enrollment
                    {
                        StudentID = 6,
                        CourseID = 3,
                        Semestar = "Winter, 2012/2013",
                        Grade = 6,
                        Year = 2012,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 48,
                        SeminalPoints = 95,
                        ProjectPoints = 67,
                        AdditionalPoints = 10,
                        FinishDate = DateTime.Parse("2012-11-12")
                    },
                    new Enrollment
                    {
                        StudentID = 6,
                        CourseID = 4,
                        Semestar = "Summer, 2015/2016",
                        Grade = 10,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 90,
                        SeminalPoints = 95,
                        ProjectPoints = 100,
                        AdditionalPoints = 5,
                        FinishDate = DateTime.Parse("2016-05-15")
                    },
                    new Enrollment
                    {
                        StudentID = 7,
                        CourseID = 5,
                        Semestar = "Summer, 2013/2014",
                        Grade = 7,
                        Year = 2014,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 60,
                        SeminalPoints = 90,
                        ProjectPoints = 75,
                        AdditionalPoints = 5,
                        FinishDate = DateTime.Parse("2014-05-13")
                    },
                    new Enrollment
                    {
                        StudentID = 7,
                        CourseID = 6,
                        Semestar = "Summer, 2015/2016",
                        Grade = 8,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 70,
                        SeminalPoints = 90,
                        ProjectPoints = 100,
                        AdditionalPoints = 0,
                        FinishDate = DateTime.Parse("2016-05-20")
                    },
                    new Enrollment
                    {
                        StudentID = 8,
                        CourseID = 7,
                        Semestar = "Summer, 2015/2016",
                        Grade = 8,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 70,
                        SeminalPoints = 90,
                        ProjectPoints = 100,
                        AdditionalPoints = 0,
                        FinishDate = DateTime.Parse("2016-05-20")
                    },
                    new Enrollment
                    {
                        StudentID = 8,
                        CourseID = 8,
                        Semestar = "2015/2016",
                        Grade = 10,
                        Year = 2016,
                        SeminalUrl = "https://feit.ukim.edu.mk/",
                        ProjectUrl = "https://e-kursevi.feit.ukim.edu.mk/",
                        ExamPoints = 90,
                        SeminalPoints = 95,
                        ProjectPoints = 100,
                        AdditionalPoints = 5,
                        FinishDate = DateTime.Parse("2016-05-15")
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
