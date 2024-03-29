﻿using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers.Api
{
    public class CoursesController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();

        }
        [Authorize]
        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId=User.Identity.GetUserId(); 
            var course=_dbContext.Courses.Single(c=> c.Id==id && c.LecturerId==userId);
            _dbContext.Courses.Remove(course);
            if (!course.IsCanceled)
                return NotFound();
            course.IsCanceled = true;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
