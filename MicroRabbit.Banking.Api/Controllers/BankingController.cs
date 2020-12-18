using MicroRabbit.Banging.Domain.Models;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroRabbit.Banking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private readonly IAccountService _accountService = default;

        public BankingController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/<BankingController>
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _accountService.GetAccounts();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountTransfer transfer)
        {
            await _accountService.TransferAsync(transfer);
            return Ok();
        }
    }
}
