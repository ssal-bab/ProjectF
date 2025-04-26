using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.FARMER_ROUTE)]
    public class FarmerController : PacketControllerBase
    {
        public FarmerController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

    }
}