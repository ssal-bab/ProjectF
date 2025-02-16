using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
	[Serializable]
	public class FarmerStatTableRow : DataTableRow
	{
        public float moveSpeedBaseValue; // �ְ� �̵� �ӵ�
        public float moveSpeedIncreaseValue; // �ְ� �̵� �ӵ�

        public float healthBaseValue; // �����ʰ� ���Ҽ� �ִ� ��
        public float healthIncreaseValue; // �����ʰ� ���Ҽ� �ִ� ��
        
        public float farmingSkillBaseValue; // �� ���� �� �� �ִ� ���� ����
        public float farmingSkillIncreaseValue; // �� ���� �� �� �ִ� ���� ����
        
        public float adventureSkillBaseValue; // Ž�迡�� �� ���� �������� ���� Ȯ��
        public float adventureSkillIncreaseValue; // Ž�迡�� �� ���� �������� ���� Ȯ��
    }

	public class FarmerStatTable : DataTable<FarmerStatTableRow> { }
}