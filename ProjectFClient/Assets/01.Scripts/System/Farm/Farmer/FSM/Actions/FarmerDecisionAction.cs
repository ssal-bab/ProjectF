using System.Collections.Generic;
using System.Linq;
using H00N.DataTables;
using H00N.Extensions;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerDecisionAction : FarmerFSMAction
    {
        // Idle | 대기시간 공식 : Random(0, (200 - iq) / 40 * 2f) + 1
        // Field | 밭 선택 규칙 : 거리순 정렬 -> count - (count * (iq - 40) / (200 - 40))회 셔플
        // iq 는 40 ~ 200 값

        private bool fieldDecided = false;
        private float idleTimer = 0f;

        public override void EnterState()
        {
            base.EnterState();

            fieldDecided = false;
            aiData.ClearTarget();
            SetIdle();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(fieldDecided)
                return;

            idleTimer -= Time.deltaTime;
            if(idleTimer > 0f)
                return;

            DecideAction();
        }

        private void DecideAction()
        {
            // 1/3 확률로 Idle, Field 정해짐
            int randomValue = Random.Range(0, 3);
            if(randomValue > 0)
                SetField();
            else
                SetIdle();
        }

        private void SetIdle()
        {
            FarmerConfigTable farmerConfigTable = DataTableManager.GetTable<FarmerConfigTable>();
            float idleDuration = Random.Range(farmerConfigTable.IdleDurationMin, farmerConfigTable.IdleDurationMax);
            idleTimer = idleDuration;
        }

        private void SetField()
        {
            // 나중에 바꿔야 함
            Farm farm = new GetBelongsFarm(Farmer.transform).currentFarm;
            if(farm == null)
            {
                SetIdle();
                return;
            }

            bool cropQueueValid = farm.CropQueue.CropQueueValid;
            List<Transform> targets = new List<Transform>();
            foreach(Field field in farm)
            {
                // IsTargetEnable이 false면 넘긴다.
                if(field.IsTargetEnable(Farmer) == false)
                    continue;

                // field의 State가 Empty, 즉 작물을 심어야 하는 단계인데 cropQueue가 비어있으면 넘긴다.
                if(field.FieldState == EFieldState.Empty && cropQueueValid == false)
                    continue;

                targets.Add(field.transform);
            }

            if(targets.Count <= 0)
            {
                SetIdle();
                return;
            }

            targets.Sort(Farmer.transform.DistanceCompare);
            aiData.PushTarget(targets[0].GetComponent<Field>());

            fieldDecided = true;
        }
    }
}
