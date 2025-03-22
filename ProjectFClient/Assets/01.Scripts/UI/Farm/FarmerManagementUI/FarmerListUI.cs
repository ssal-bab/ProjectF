using System;
using System.Collections;
using System.Collections.Generic;
using ProjectF.UI;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class FarmerListUI : MonoBehaviourUI
    {
        private OrderType currentOrderType = OrderType.Ascending;
        private ClassificationType currentClasification = ClassificationType.Acquisition;

        public OrderType ChangeOrder()
        {
            currentOrderType = currentOrderType == OrderType.Ascending ? OrderType.Descending : OrderType.Ascending;
            return currentOrderType;
        }

        public ClassificationType ChangeClassification(ClassificationType classificationType)
        {
            currentClasification = classificationType;
            return currentClasification;
        }
    }
}
