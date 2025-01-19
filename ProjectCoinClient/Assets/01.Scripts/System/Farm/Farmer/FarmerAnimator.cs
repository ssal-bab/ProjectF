using UnityEngine;

namespace ProjectCoin.Farms
{
    public partial class FarmerAnimator : MonoBehaviour
    {
        private Animator animator = null;
        private int currentHash = 0;

        private void Awake()
        {
            currentHash = IS_IDLE_HASH;
            animator = GetComponent<Animator>();
        }

        private void ChangeState(int hash)
        {
            if(currentHash == hash)
                return;

            animator.SetBool(currentHash, false);
            currentHash = hash;
            animator.SetBool(currentHash, true);
        }
    }
}
