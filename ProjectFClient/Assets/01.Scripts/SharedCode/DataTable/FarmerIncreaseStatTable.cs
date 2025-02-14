using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class FarmerIncreaseStatTableRow : DataTableRow
    {
        public float moveSpeed; // �ְ� �̵� �ӵ�
        public float health; // �����ʰ� ���Ҽ� �ִ� ��
        public float farmingSkill; // �� ���� �� �� �ִ� ���� ����
        public float adventureSkill; // Ž�迡�� �� ���� �������� ���� Ȯ��
    }

    public class FarmerIncreaseStatTable : DataTable<FarmerIncreaseStatTableRow> { }
}
