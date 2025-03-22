using System.Collections;
using System.Collections.Generic;
using H00N.Extensions;
using H00N.Resources.Pools;
using ProjectF.Farms;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class FarmerListPopupUI : PoolableBehaviourUI
    {
        [SerializeField] private FarmerManagement _farmerManagement;
        // 외부 클래스로 빼야 할 항목 : 일꾼 목록 Element, 일꾼 Info Popup
        // 해당 클래스 내에서 구현해야 할 항목 : 
        public new void Initialize()
        {
            base.Initialize();
        }

        public void SalesFarmer()
        {

        }

        // 분류 항목 기준 변경
        public void ChangeElementClassification()
        {

        }

        // 오름차순 내림차순 변경
        public void ChangeOrder()
        {

        }

        protected override void Release()
        {
            base.Release();
        }
    }
}
