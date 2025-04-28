// using System;

// namespace ProjectF.UI.Farms
// {
//     public class FarmerGainPopupUICallbackContainer : UICallbackContainer
//     {
//         // <TargetFarmerUUID, ChangedName>
//         public Action<string, string> ChangeNameCallback = null;
        
//         // <TargetFarmerUUID>
//         public Action<string> SellFarmerCallback = null;
        
//         // <TargetFarmerID>
//         public Action<string> OpenCollectionBookCallback = null;

//         public FarmerGainPopupUICallbackContainer(Action<string, string> changeNameCallback, Action<string> sellFarmerCallback, Action<string> openCollectionBookCallback)
//         {
//             ChangeNameCallback = changeNameCallback;
//             SellFarmerCallback = sellFarmerCallback;
//             OpenCollectionBookCallback = openCollectionBookCallback;

//             /*
//             (uuid, name) => {
//                 Debug.Log($"Change Farmer Name!! uuid : {uuid}, name : {name}");

//             }
//             */
//         }
//     }
// }