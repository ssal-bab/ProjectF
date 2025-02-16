using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class FarmerGainPopupUI : MonoBehaviourUI
    {
        [SerializeField] Image farmerIconImage = null;
        [SerializeField] TMP_InputField farmerNameInputField = null;
        [SerializeField] FarmerStatUI staminaStatUI = null;
        [SerializeField] FarmerStatUI moveSpeedStatUI = null;
        [SerializeField] FarmerStatUI farmingSkillStatUI = null;
        [SerializeField] FarmerStatUI adventureSkillStatUI = null;

        private string farmerUUID = null;
        private FarmerGainPopupUICallbackContainer callbackContainer = null;

        public void Initialize(string farmerUUID, int farmerID, FarmerGainPopupUICallbackContainer callbackContainer)
        {
            base.Initialize();
            this.farmerUUID = farmerUUID;
            this.callbackContainer = callbackContainer;

            FarmerTableRow farmerTableRow = DataTableManager.GetTable<FarmerTable>().GetRow(farmerID);
            if(farmerTableRow == null)
                return;

            FarmerStatTableRow farmerStatTableRow = DataTableManager.GetTable<FarmerStatTable>().GetRow(farmerID);
            if(farmerStatTableRow == null)
                return;

            RefreshUI(farmerTableRow, farmerStatTableRow);
        }

        private void RefreshUI(FarmerTableRow farmerTableRow, FarmerStatTableRow farmerStatTableRow)
        {
            farmerIconImage.sprite = ResourceUtility.GetFarmerIcon(farmerTableRow.id);
            farmerNameInputField.text = farmerTableRow.nameLocalKey;
            staminaStatUI.Initialize(EFarmerStatType.Health, farmerStatTableRow.healthBaseValue);
            moveSpeedStatUI.Initialize(EFarmerStatType.MoveSpeed, farmerStatTableRow.moveSpeedBaseValue);
            farmingSkillStatUI.Initialize(EFarmerStatType.FarmingSkill, farmerStatTableRow.farmingSkillBaseValue);
            adventureSkillStatUI.Initialize(EFarmerStatType.AdventureSkill, farmerStatTableRow.adventureSkillBaseValue);
        }

        public void OnTouchSellButton()
        {
            callbackContainer.SellFarmerCallback?.Invoke(farmerUUID);
        }

        public void OnTouchCollectionBookButton()
        {
            callbackContainer.OpenCollectionBookCallback?.Invoke(farmerUUID);
        }

        public void OnTouchCloseButton()
        {
            PoolManager.DespawnAsync(GetComponent<PoolReference>()).Forget();
        }

        public void OnChangeName(string name)
        {
            callbackContainer.ChangeNameCallback?.Invoke(farmerUUID, name);
        }
    }
}