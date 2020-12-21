using MicroRabbit.Transfer.Application.Services.Abstractions;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroRabbit.Transfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService = default;

        public TransferController(ITransferService transferServ)
        {
            _transferService = transferServ;
        }

        // GET: api/<TransferController>
        [HttpGet]
        public async Task<IEnumerable<TransferLog>> Get()
        {
            return await _transferService.GetTransferLogs();
        }
    }
}
