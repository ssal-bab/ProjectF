using System;
using System.Collections;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class NestEggInfoUI : MonoBehaviourUI
    {
        [Header("Default Group")]
        [SerializeField] Image eggIconImage = null;

        [Header("Hatching Finish Group")]
        [SerializeField] GameObject hatchingFinishGroupObject = null;
        
        [Header("Remain Time Group")]
        [SerializeField] GameObject remainTimeGroupObject = null;
        [SerializeField] RectTransform remainTimeSlider = null;
        [SerializeField] TMP_Text remainTimeText = null;

        private const float UPDATE_DELAY_SECONDS = 0.01f;

        public void Initialize(EggHatchingData eggHatchingData)
        {
            base.Initialize();
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

            eggIconImage.sprite = ResourceUtility.GetEggIcon(tableRow.id);
            eggIconImage.color = Color.white;

            TimeSpan elapsedTimeSpan = GameInstance.ServerTime - eggHatchingData.hatchingStartTime;
            float remainTime = (float)(tableRow.hatchingTime - elapsedTimeSpan.TotalSeconds);
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
                    remainTimeText.text = StringUtility.GetTimeString(TimeSpan.FromSeconds(lastTextUpdateSeconds), StringUtility.ETimeStringType.HoursMinutesSeconds);
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
            eggIconImage.sprite = null;
            eggIconImage.color = new Color(0, 0, 0, 0);
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