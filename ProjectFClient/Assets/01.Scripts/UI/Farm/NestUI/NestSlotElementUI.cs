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
    public class NestSlotElementUI : MonoBehaviourUI
    {
        [SerializeField] GameObject hatchingFinishGroupObject = null;
        [SerializeField] GameObject remainTimeGroupObject = null;
        [SerializeField] Slider remainTimeSlider = null;
        [SerializeField] TMP_Text remainTimeText = null;

        private const float UPDATE_DELAY_SECONDS = 1;

        public void Initialize(EggHatchingData eggHatchingData)
        {
            if(eggHatchingData == null)
            {
                hatchingFinishGroupObject.SetActive(false);
                remainTimeGroupObject.SetActive(false);
                return;
            }

            TimeSpan remainTime = GameInstance.ServerTime - eggHatchingData.hatchingStartTime;
            bool hatchingFinish = remainTime.TotalSeconds <= 0;
            SetUI(hatchingFinish);

            if(hatchingFinish == false)
                StartCoroutine(UpdateRoutine(eggHatchingData));
        }

        private IEnumerator UpdateRoutine(EggHatchingData eggHatchingData)
        {
            EggTableRow tableRow = DataTableManager.GetTable<EggTable>()[eggHatchingData.eggID];
            if(tableRow == null)
                yield break;

            YieldInstruction updateDelay = new WaitForSeconds(UPDATE_DELAY_SECONDS);

            while(true)
            {
                TimeSpan remainTime = GameInstance.ServerTime - eggHatchingData.hatchingStartTime;
                remainTimeSlider.value = (float)(remainTime.TotalSeconds / tableRow.hatchingTime);
                remainTimeText.text = StringUtility.GetTimeString(remainTime, StringUtility.ETimeStringType.HoursMinutesSeconds);

                bool hatchingFinish = remainTime.TotalSeconds <= 0;
                if (hatchingFinish)
                {
                    SetUI(hatchingFinish);
                    yield break;
                }

                yield return updateDelay;
            }
        }

        private void SetUI(bool hatchingFinish)
        {
            hatchingFinishGroupObject.SetActive(hatchingFinish);
            remainTimeGroupObject.SetActive(!hatchingFinish);
        }
    }
}