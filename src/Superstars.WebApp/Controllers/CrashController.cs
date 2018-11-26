﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrashGameMath;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Superstars.WebApp.Authentication;
using Superstars.WebApp.Services;

namespace Superstars.WebApp.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class CrashController : Controller
    {
        private CrashBuilder _crash;

        public CrashController(CrashBuilder crash, CrashService service)
        {
            _crash = crash;
            
        }
        [HttpGet("getNextCrash")]
        public async Task<IActionResult> GetNextCrash()
        {
             return Ok();
        }
    }
}
