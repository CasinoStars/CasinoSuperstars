﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Superstars.DAL;
using Superstars.WebApp.Authentication;
using Superstars.WebApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Superstars.Wallet;
using NBitcoin;
using QBitNinja.Client;
using System.Collections.Generic;

namespace Superstars.WebApp.Controllers
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
	public class WalletController : Controller
	{
		WalletGateway _walletGateway;

		public WalletController(WalletGateway walletGateway)
		{
			_walletGateway = walletGateway;
		}

		/// <summary>
		/// Add Real or Fake Coins for player
		/// </summary>
		/// <returns></returns>
		[HttpPost("AddCoins")]
		public async Task<IActionResult> AddFakeCoins([FromBody] WalletViewModel model)
		{
			int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			Result result = await _walletGateway.AddCoins(userId, model.MoneyType, model.FakeCoins,0,0);
			return this.CreateResult(result);
		}

		[HttpPost("{pot}/creditFakePlayer")]
        public async Task<IActionResult> CreditPlayerFake(int pot)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Result result = await _walletGateway.AddCoins(userId, 2, pot, pot,0);
            return this.CreateResult(result);
        }

        [HttpPost("{pot}/creditBTCPlayer")]
        public async Task<IActionResult> CreditPlayerBTC(int pot)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Result result = await _walletGateway.AddCoins(userId, 1, 0, 0, pot);
            return this.CreateResult(result);
        }

        [HttpPost("{pot}/withdrawFakeBank")]
        public async Task<IActionResult> WithdrawFakeBankRoll(int pot)
        {
            Result result = await _walletGateway.InsertInBankRoll(0, -pot);
            return this.CreateResult(result);
        }

        [HttpPost("{pot}/withdrawBtcBank")]
        public async Task<IActionResult> WithdrawBTCBankRoll(int pot)
        {
            Result result = await _walletGateway.InsertInBankRoll(-pot, 0);
            return this.CreateResult(result);
        }

        [HttpGet("BTCBankRoll")]
        public async Task<decimal> GetBTCBankRoll()
        {
            Result<int> result = await _walletGateway.GetBTCBankRoll();
            BitcoinSecret privateKey = new BitcoinSecret("cTSNviQWYnSDZKHvkjwE2a7sFW47sNoGhR8wjqVPb6RbwqH1pzup"); //PRIVATE KEY OF ALL'IN BANKROLL
            int onBlockchain = informationSeeker.HowMuchCoinInWallet(privateKey, new QBitNinjaClient(Network.TestNet)); //AMOUNT BTC ON BLOCKCHAIN
            Result<decimal> allCredit = await _walletGateway.GetAllCredit();
            decimal BTCBank;
            if (allCredit.Content <= 0)
            {
                BTCBank = onBlockchain - allCredit.Content;
            }
            else
            {
                BTCBank = onBlockchain + allCredit.Content;
            }
            return BTCBank;
        }

        [HttpGet("FakeBankRoll")]
        public async Task<IActionResult> GetFakeBankRoll()
        {
            Result<int> result = await _walletGateway.GetFakeBankRoll();
            return this.CreateResult(result);
        }

        [HttpGet("TrueBalance")]
        public async Task<int> GetTrueBalance()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Result<WalletData> result1 = await _walletGateway.GetPrivateKey(userId);
            BitcoinSecret privateKey = new BitcoinSecret("cTSNviQWYnSDZKHvkjwE2a7sFW47sNoGhR8wjqVPb6RbwqH1pzup");
            if (userId == 0)
            {
                 privateKey = new BitcoinSecret("cTSNviQWYnSDZKHvkjwE2a7sFW47sNoGhR8wjqVPb6RbwqH1pzup");
            }
            else
            {
                 privateKey = new BitcoinSecret(result1.Content.PrivateKey/*"cTSNviQWYnSDZKHvkjwE2a7sFW47sNoGhR8wjqVPb6RbwqH1pzup"*/);
            }
            int onBlockchain = informationSeeker.HowMuchCoinInWallet(privateKey, new QBitNinjaClient(Network.TestNet));
            Result<int> credit = await _walletGateway.GetCredit(userId);
            int realBalance = onBlockchain + credit.Content;
            return realBalance;
        }

        [HttpGet("FakeBalance")]
        public async Task<IActionResult> GetFakeBalance()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Result<WalletData> result = await _walletGateway.GetFakeBalance(userId);
            return this.CreateResult(result);
        }

        [HttpGet("GetAddress")]
        public async Task<string> GetAddress()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Result<WalletData> result = await _walletGateway.GetPrivateKey(userId);
            BitcoinSecret privateKey = new BitcoinSecret(result.Content.PrivateKey);
            string Address = privateKey.GetAddress().ToString();
            return Address;
        }

        [HttpPost("Withdraw")]
        public  async Task<List<string>> WithdrawPlayer([FromBody] WalletViewModel WalletViewModel)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Result<WalletData> result1 = await _walletGateway.GetPrivateKey(userId);
            BitcoinSecret privateKey = new BitcoinSecret(/*result1.Content.PrivateKey*/"cTSNviQWYnSDZKHvkjwE2a7sFW47sNoGhR8wjqVPb6RbwqH1pzup");
            QBitNinjaClient client = new QBitNinjaClient(Network.TestNet);
            BitcoinAddress destinationAddress = BitcoinAddress.Create(WalletViewModel.DestinationAddress,Network.TestNet);
            var transaction = TransactionMaker.MakeATransaction(privateKey,destinationAddress, WalletViewModel.AmountToSend, 50000, 6, client);
            List<string> response =  TransactionMaker.BroadCastTransaction(transaction,client);

            return response;
        }
    }
}