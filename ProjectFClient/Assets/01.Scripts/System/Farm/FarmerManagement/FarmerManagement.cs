using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectF.Farms
{
    public class FarmerManagement : MonoBehaviour
    {
        /// <summary>
        /// ��� ���� ���̺� SO list
        /// </summary>
        [SerializeField] private List<object> increaseFarmingStatTableList = new();
        private Dictionary<int, object> increaseFarmingStatTableDictionary;

        private void Start()
        {
            // increaseFarmingStatTableList.ForEach(x => increaseFarmingStatTableDictionary.Add(x.id, x));
        }

        /// <summary>
        /// ���� ����� �� �������� �ϲ� ���� �缳��
        /// </summary>
        public void GenerateFarmerStat()
        {
            /// <summary>
            /// �������� �ϲ۵� ������ ���̺��� ��𼱰� ������
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
            /// 3. FarmerStatSO�� �� �����ϱ�
            /// </summary>
        }

        /// <summary>
        /// �ϲ� ���� ����, target = ������ �ϲ�, level = ��ǥ ����
        /// </summary>
        public void ChangeAdventureLevel(Farmer target, int level)
        {
            /// <summary>
            /// 1. Ÿ���� ID �̾ƿͼ� ���� ������ ���̺� ã��
            /// 2. ������ ���̺��� ���� �Լ��� ������ ���ȵ� Ʃ�÷� �޾ƿ���
            /// 3. FarmerStatSO�� �� �����ϱ�
            /// </summary>
        }
    }
}
