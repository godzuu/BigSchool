﻿using BigSchool.DTOs;
using BigSchool.Models;

using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public FollowingsController()
        {
            _dbContext = new ApplicationDbContext();

        }
        [System.Web.Mvc.HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var userId = User.Identity.GetUserId();
            if (_dbContext.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == followingDto.FolloweeId))
                return BadRequest("The Attendance already exists");
            var folowing  = new Following

            {
                FollowerId = userId,
            FolloweeId = followingDto.FolloweeId,
            };
            _dbContext.Followings.Add(folowing);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
