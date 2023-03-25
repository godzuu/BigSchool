﻿using BigSchool.Models;
using BigSchool.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Create()
        {
            Console.WriteLine(1);
            var viewModel = new CourseViewModel
            {
              Categories =_dbConText.Categories.ToList(),
              Heading="Add Course"
            };
            return View("Create", viewModel);
        }
        private readonly ApplicationDbContext  _dbConText;
        public CoursesController()
        {
            _dbConText = new ApplicationDbContext();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewmodel) 
        {
            if (!ModelState.IsValid)
            {
                viewmodel.Categories = _dbConText.Categories.ToList();
                return View("Create",viewmodel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                Datetime = viewmodel.GetDateTime(),
                CategoryId = viewmodel.Category,
                    Place= viewmodel.Place
            };
            _dbConText.Courses.Add(course);
            _dbConText.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId=User.Identity.GetUserId();
            var courses=_dbConText.Attendances
                .Where(a=>a.AttendeeId==userId)
                .Select(a=>a.Course)
                .Include(l=>l.Lecturer)
                .Include(l=>l.Category)
                .ToList();
            var viewModel = new CourseViewModel
            {
                UpcomingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated,
            };
            return View(viewModel);
        }
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbConText.Courses
                .Where(c => c.LecturerId == userId && c.Datetime > DateTime.Now)
                
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
          
            return View(courses);
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userid=User.Identity.GetUserId();
            var course=_dbConText.Courses.Single(c=>c.Id == id && c.LecturerId==userid);
            var viewModel = new CourseViewModel
            {
                Categories = _dbConText.Categories.ToList(),
                Date = course.Datetime.ToString("dd/M/yyyy"),
                Time = course.Datetime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                Heading="Edit Course",
                Id= course.Id,
            };
            return View("Create", viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbConText.Categories.ToList();
                return View("Create", viewModel);
            }
            var userId=User.Identity.GetUserId();
            var course = _dbConText.Courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userId);
            course.Place= viewModel.Place;
            course.Datetime=viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;
            _dbConText.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}