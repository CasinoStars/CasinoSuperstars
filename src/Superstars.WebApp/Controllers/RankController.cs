﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Superstars.DAL;
using Superstars.WebApp.Authentication;
using Superstars.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Superstars.WebApp.Controllers
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
	public class RankController : Controller
	{
		RankGateway _rankGateway;

		public RankController(RankGateway rankGateway)
		{
			_rankGateway = rankGateway;
		}

		[HttpGet("PseudoList")]
		public async Task<IEnumerable<string>> GetPseudoList()
		{
			IEnumerable<string> names = await _rankGateway.PseudoList();
			return names;
		}

		[HttpGet("PlayerProfit")]
		public async Task<IEnumerable<int>> GetPlayerProfitList(string pseudo)
		{
			IEnumerable<int> profit = await _rankGateway.GetPlayerProfitList(pseudo);
			return profit;
		}

		[HttpGet("PlayersProfitSorted")]
		public List<int> GetPlayersProfitSorted(RankService rankService)
		{
			List<int> profits = rankService.Profit;
			return profits;
		}

		[HttpGet("PlayersUserNameSorted")]
		public List<string> GetPlayersUserNameSorted(RankService rankService)
		{
			List<string> userName = rankService.UserName;
			return userName;
		}

	}
}
