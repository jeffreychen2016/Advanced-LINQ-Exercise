using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using LINQ_Practice.Models;
using System.Linq;

namespace LINQ_Practice
{
    [TestClass]
    public class LINQ_Practice_MethodChaining
    {
        public List<Cohort> PracticeData { get; set; }
        public CohortBuilder CohortBuilder { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            CohortBuilder = new CohortBuilder();
            PracticeData = CohortBuilder.GenerateCohorts();
        }

        [TestCleanup]
        public void TearDown()
        {
            CohortBuilder = null;
            PracticeData = null;
        }

        [TestMethod]
        public void GetAllCohortsWithZacharyZohanAsPrimaryOrJuniorInstructor()
        {
            var ActualCohorts = PracticeData.Where(cohort => (cohort.PrimaryInstructor.FirstName == "Zachary"
            && cohort.PrimaryInstructor.LastName == "Zohan")
            || cohort.JuniorInstructors.Any(junior => (junior.FirstName == "Zachary" && junior.LastName == "Zohan")));
        }

        [TestMethod]
        public void GetAllCohortsWhereFullTimeIsFalseAndAllInstructorsAreActive()
        {
            var ActualCohorts = PracticeData.Where(cohort => !cohort.FullTime && cohort.PrimaryInstructor.Active && cohort.JuniorInstructors.All(junior => junior.Active))/*FILL IN LINQ EXPRESSION*/.ToList();
            CollectionAssert.AreEqual(ActualCohorts, new List<Cohort> { CohortBuilder.Cohort1 });
        }

        [TestMethod]
        public void GetAllCohortsWhereAStudentOrInstructorFirstNameIsKate()
        {
            var ActualCohorts = PracticeData.Where(cohort => cohort.JuniorInstructors.Any(junior => junior.FirstName == "Kate") 
                                                            || cohort.PrimaryInstructor.FirstName == "Kate" 
                                                            || cohort.Students.Any(student => student.FirstName == "kate")).ToList();



            CollectionAssert.AreEqual(ActualCohorts, new List<Cohort> { CohortBuilder.Cohort1, CohortBuilder.Cohort3, CohortBuilder.Cohort4 });
        }

        [TestMethod]
        public void GetOldestStudent()
        {
            // this does not work
            // var student = PracticeData.SelectMany(cohort => cohort.Students).Min(eachStudent => eachStudent.Birthday)/*FILL IN LINQ EXPRESSION*/;

            // this works
            //var student = (from eachStudent in PracticeData.SelectMany(cohort => cohort.Students)
            //               orderby eachStudent.Birthday ascending
            //               select eachStudent).First();
            var student = PracticeData.SelectMany(cohort => cohort.Students).OrderBy(eachStudent => eachStudent.Birthday).First();
            Assert.AreEqual(student, CohortBuilder.Student18);
        }

        [TestMethod]
        public void GetYoungestStudent()
        {
            var student = PracticeData.SelectMany(cohort => cohort.Students).OrderByDescending(eachStudent => eachStudent.Birthday).First()/*FILL IN LINQ EXPRESSION*/;
            Assert.AreEqual(student, CohortBuilder.Student3);
        }

        [TestMethod]
        public void GetAllInactiveStudentsByLastName()
        {
            var ActualStudents = PracticeData.SelectMany(cohort => cohort.Students).Where(student => !student.Active)/*FILL IN LINQ EXPRESSION*/.ToList();
            CollectionAssert.AreEqual(ActualStudents, new List<Student> { CohortBuilder.Student2, CohortBuilder.Student11, CohortBuilder.Student12, CohortBuilder.Student17 });
        }
    }
}
