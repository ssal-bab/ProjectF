using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectF.Farms
{
    public class FarmerManagement : MonoBehaviour
    {
        /// <summary>
        /// 농사 스탯 테이블 SO list
        /// </summary>
        [SerializeField] private List<object> increaseFarmingStatTableList = new();
        private Dictionary<int, object> increaseFarmingStatTableDictionary;

        private void Start()
        {
            // increaseFarmingStatTableList.ForEach(x => increaseFarmingStatTableDictionary.Add(x.id, x));
        }

        /// <summary>
        /// 게임 재시작 시 보유중인 일꾼 스탯 재설정
        /// </summary>
        public void GenerateFarmerStat()
        {
            /// <summary>
            /// 보유중인 일꾼들 데어터 테이블을 어디선가 가져옴
            /// foreach loop를 돌며 데이터 테이블을 참조해 일꾼들의 레벨당 스탯을 재설정
            /// 여기서 농사 스탯, 탐험 스탯 전부 초기화 함
            /// </summary>
        }

        /// <summary>
        /// 일꾼 레벨 변경, target = 변경할 일꾼, level = 목표 레벨
        /// </summary>
        public void ChangeFarmingLevel(Farmer target, int level)
        {
            /// <summary>
            /// 1. 타겟의 ID 뽑아와서 스탯 데이터 테이블 찾기
            /// 2. 데이터 테이블의 내장 함수로 레벨의 스탯들 튜플로 받아오기
            /// 3. FarmerStatSO에 값 수정하기
            /// </summary>
        }

        /// <summary>
        /// 일꾼 레벨 변경, target = 변경할 일꾼, level = 목표 레벨
        /// </summary>
        public void ChangeAdventureLevel(Farmer target, int level)
        {
            /// <summary>
            /// 1. 타겟의 ID 뽑아와서 스탯 데이터 테이블 찾기
            /// 2. 데이터 테이블의 내장 함수로 레벨의 스탯들 튜플로 받아오기
            /// 3. FarmerStatSO에 값 수정하기
            /// </summary>
        }
    }
}
