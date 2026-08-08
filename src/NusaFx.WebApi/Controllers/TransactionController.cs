using Microsoft.AspNetCore.Mvc;
using NusaFx.Application.Common.Models;
using NusaFx.Application.Interfaces;

namespace NusaFx.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var results = await _transactionService.GetTransactionsAsync(pageNumber, pageSize);

            if (results.Equals(null))
            {
                return BadRequest(results.ErrorMessage);
            }

            return Ok(
                new
                {
                    items = results.Items,
                    totalCount = results.TotalCount,
                    pageNumber = results.PageNumber,
                    pageSize = results.PageSize,
                }
            );
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequest request)
        {
            var result = await _transactionService.CreateTransactionAsync(
                request.Description,
                request.Amount,
                request.Currency
            );

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [HttpPut("transactions/{id}")]
        public async Task<IActionResult> UpdateTransaction(
            Guid id,
            [FromBody] TransactionRequest request
        )
        {
            var result = await _transactionService.UpdateTransactionAsync(
                id,
                request.Description,
                request.Amount,
                request.Currency
            );

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("transactions/{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("transactions/{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var result = await _transactionService.DeleteTransactionAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }
    }
}
