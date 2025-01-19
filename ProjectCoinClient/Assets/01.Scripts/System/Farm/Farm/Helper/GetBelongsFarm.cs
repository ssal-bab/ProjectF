using UnityEngine;

namespace ProjectCoin.Farms.Helpers
{
    public struct GetBelongsFarm
    {
        public Farm currentFarm;

        public GetBelongsFarm(Transform transform)
        {
            currentFarm = null;

            Collider2D detected = Physics2D.OverlapCircle(transform.position, 0.1f, (int)ELayer.FarmLayer);
            if(detected == null)
                return;

            currentFarm = detected.GetComponent<Farm>();
        }
    }
}