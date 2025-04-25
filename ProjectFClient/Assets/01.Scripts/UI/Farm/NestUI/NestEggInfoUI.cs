using System;
using System.Collections;
using H00N.DataTables;
using H00N.Resources;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectF.StringUtility;

namespace ProjectF.UI.Farms
{
    public class NestEggInfoUI : MonoBehaviourUI
    {
        [Header("Default Group")]
        [SerializeField] Image eggIconImage = null;
        [SerializeField] AddressableAsset<Sprite> emptyImage = null;

        [Header("Hatching Finish Group")]
        [SerializeField] GameObject hatchingFinishGroupObject = null;
        
        [Header("Remain Time Group")]
        [SerializeField] GameObject remainTimeGroupObject = null;
        [SerializeField] RectTransform remainTimeSlider = null;
        [SerializeField] TMP_Text remainTimeText = null;

        private const float UPDATE_DELAY_SECONDS = 0.01f;

        public async void Initialize(EggHatchingData eggHatchingData)
        {
            base.Initialize();
            await emptyImage.InitializeAsync();
            StopAllCoroutines();

            if (eggHatchingData == null)
            {
                EnactiveUI();
                return;
            }

            EggTableRow tableRow = DataTableManager.GetTable<EggTable>()[eggHatchingData.eggID];
            if (tableRow == null)
            {
                EnactiveUI();
                return;
            }

            new SetSprite(eggIconImage, ResourceUtility.GetEggIconKey(tableRow.id));

            float remainTime = (float)(eggHatchingData.hatchingFinishTime - GameInstance.ServerTime).TotalSeconds;
            bool hatchingFinish = remainTime <= 0;
            SetUI(hatchingFinish);

            if (hatchingFinish == false)
                StartCoroutine(UpdateRoutine(remainTime, tableRow.hatchingTime));
        }

        private IEnumerator UpdateRoutine(float remainTime, float hatchingTime)
        {
            YieldInstruction updateDelay = new WaitForSeconds(UPDATE_DELAY_SECONDS);
            int lastTextUpdateSeconds = 0;
            while (true)
            {
                Vector2 anchorMax = remainTimeSlider.anchorMax;
                anchorMax.x = Mathf.Max((hatchingTime - remainTime) / hatchingTime, 0);
                remainTimeSlider.anchorMax = anchorMax;

                int currentSeconds = (int)remainTime;
                if(currentSeconds != lastTextUpdateSeconds)
                {
                    lastTextUpdateSeconds = currentSeconds;
                    remainTimeText.text = GetTimeString(ETimeStringType.HoursMinutesSeconds, TimeSpan.FromSeconds(lastTextUpdateSeconds));
                }

                bool hatchingFinish = remainTime <= 0;
                if (hatchingFinish)
                {
                    SetUI(hatchingFinish);
                    yield break;
                }

                DateTime lastUpdateTime = GameInstance.ServerTime;
                yield return updateDelay;
                remainTime -= (float)(GameInstance.ServerTime - lastUpdateTime).TotalSeconds;
            }
        }

        private void EnactiveUI()
        {
            eggIconImage.sprite = emptyImage.Asset;
            hatchingFinishGroupObject.SetActive(false);
            remainTimeGroupObject.SetActive(false);
        }

        private void SetUI(bool hatchingFinish)
        {
            hatchingFinishGroupObject.SetActive(hatchingFinish);
            remainTimeGroupObject.SetActive(!hatchingFinish);
        }
    }
}