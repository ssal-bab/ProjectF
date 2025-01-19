using Microsoft.AspNetCore.Mvc;
using ProjectCoin.Networks.Payloads;

namespace ProjectCoin.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.FARM_ROUTE)]
    public class FarmController : ControllerBase
    {
        [HttpPost(PlantRequest.POST)]
        public async Task<ActionResult<PlantResponse>> PlantCropRequestPost([FromBody]PlantRequest req)
        {
            // db write

            PlantResponse response = new PlantResponse() {
                networkResult = ENetworkResult.Success,
                cropID = req.cropID
            };

            return response;
        }

        [HttpPost(HarvestRequest.POST)]
        public async Task<ActionResult<HarvestResponse>> HarvestRequestPost([FromBody]HarvestRequest req)
        {
            // db write

            HarvestResponse response = new HarvestResponse() {
                networkResult = ENetworkResult.Success,
                productCropID = 0
            };

            return response;
        }
    }
}
