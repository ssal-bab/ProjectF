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
            if(randomValue > 1)
                SetField();
            else
                SetIdle();
        }

        private void SetIdle()
        {
            FarmerConfigTable farmerConfigTable = DataTableManager.GetTable<FarmerConfigTable>();
            float idleDuration = Random.Range(farmerConfigTable.IdleDurationMin(), farmerConfigTable.IdleDurationMax());
            idleTimer = idleDuration;
        }

        private void SetField()
        {
            // 나중에 바꿔야 함
            List<Transform> targets = FindObjectsOfType<FarmerTargetableBehaviour>()
                .Where(i => i.TargetEnable && !i.IsWatched)
                .Select(i => i.transform).ToList();

            if(targets.Count <= 0)
            {
                SetIdle();
                return;
            }

            targets.Sort(transform.DistanceCompare);
            aiData.PushTarget(targets[0].GetComponent<FarmerTargetableBehaviour>());

            fieldDecided = true;
        }
    }
}
