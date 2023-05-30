using api.Dto;
using api.Model;
using api.Request;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TightsController : ControllerBase
    {
        private readonly AccessMongoTights _accessMongoTights;
        public TightsController(AccessMongoTights accessMongoTights)
        {
            _accessMongoTights = accessMongoTights;
        }

        [HttpGet("Tights")]
        public async Task<IEnumerable<FlattenTight>> GetTightsAsync(CancellationToken cancellation)
        {
            return await _accessMongoTights.GetFlattenTights(cancellation);
        }

        [HttpGet("Analisys")]
        public async Task<StatiscalResult> GetAnalisys(CancellationToken cancellation)
        {
            return await _accessMongoTights.GetAnalisys(cancellation);
        }

        [HttpGet("Tights2")]
        public async Task<IEnumerable<FlattenTight>> GetTight2([FromQuery] FlattenDataRequest request, CancellationToken cancellation)
            => await _accessMongoTights.GetFlatten2(request,cancellation);
        [HttpGet("Tights3")]
        public async Task<IEnumerable<FlattenTight>> GetTight3([FromQuery] FlattenDataRequest request, CancellationToken token)
            => await _accessMongoTights.GetFlatten3(request,token);
        
        [HttpGet("Histogram")]
        public async Task<StatiscalResult> GetHistogram([FromQuery]  HistogramRequest request, CancellationToken cancellation)
            => await _accessMongoTights.GetHistogram(request,cancellation);

    }
}
