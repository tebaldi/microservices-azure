using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchT.Services.Inventory.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArchT.Services.Inventory.Controllers
{
    [ApiController, Route("api/v1/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Route("products")]
        public async Task<QueryResponse<IEnumerable<ProductData>>> GetProducts([FromQuery]GetProductsRequest request) => await _mediator.Send(request);

        [HttpGet, Route("products/{id}")]
        public async Task<QueryResponse<ProductData>> FindProduct([FromQuery]FindProductRequest request) => await _mediator.Send(request);

        [HttpPost, Route("products/{id}/update-name")]
        public async Task<CommandResponse> UpdateProductName([FromBody]UpdateProductNameRequest request) => await _mediator.Send(request);

        [HttpPost, Route("products/{id}/increase-stock")]
        public async Task<CommandResponse> IncreaseProductStock([FromBody]IncreaseProductStockRequest request) => await _mediator.Send(request);

        [HttpPost, Route("products/{id}/decrease-stock")]
        public async Task<CommandResponse> DecreaseProductStock([FromBody]DecreaseProductStockRequest request) => await _mediator.Send(request);
    }
}
