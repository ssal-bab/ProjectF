using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectF.Farms
{
    public class FarmerManagement : MonoBehaviour
    {
        private readonly FarmingStatCalculator fStatCalculator = new ();
        private readonly AdventureStatCalculator aStatCalculator = new ();

        /// <summary>
        /// ��� ���� ���̺� SO list
        /// </summary>
        [SerializeField] private List<object> farmingStatTableList = new();
        private Dictionary<int, object> farmingStatTableDictionary;

        private void Start()
        {
            // farmingStatTableList.ForEach(x => farmingStatTableDictionary.Add(x.ID, x));
        }

        /// <summary>
        /// ���� ����� �� �������� �ϲ� ���� �缳��
        /// </summary>
        public void GenerateFarmerStat()
        {
            /// <summary>
            /// �ϲ۵� ����Ʈ�� �������� ������
            /// foreach loop�� ���� ������ ���̺��� ������ �ϲ۵��� ������ ������ �缳��
            /// ���⼭ ��� ����, Ž�� ���� ���� �ʱ�ȭ ��
            /// </summary>
        }

        /// <summary>
        /// �ϲ� ���� ����, target = ������ �ϲ�, level = ��ǥ ����
        /// </summary>
        public void ChangeFarmingLevel(Farmer target, int level)
        {
            /// <summary>
            /// 1. Ÿ���� ID �̾ƿͼ� ���� ������ ���̺� ã��
            /// 2. ������ ���̺��� ���� �Լ��� ������ ���ȵ� Ʃ�÷� �޾ƿ���
            /// 3. FarmerStatSO�� ������̾� �ǵ���� �� �����ϱ�
            /// </summary>
        }
    }
}
