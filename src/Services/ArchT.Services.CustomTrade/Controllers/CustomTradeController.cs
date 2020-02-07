using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static DataContracts;
using static QueryContracts;

namespace ArchT.Services.CustomTrade.Controllers
{
    [ApiController, Route("api/v1/customtrade")]
    public class CustomTradeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomTradeController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Route("orders")]
        public async Task<IEnumerable<OrderData>> GetOrders([FromQuery]GetOdersRequest request) => await _mediator.Send(request);

        [HttpGet, Route("orders/{id}")]
        public async Task<OrderData> FindOrder([FromQuery]FindOrderRequest request) => await _mediator.Send(request);
    }
}
