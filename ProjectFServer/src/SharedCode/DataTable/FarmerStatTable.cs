using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
	[Serializable]
	public partial class FarmerStatTableRow : DataTableRow
	{
        public float moveSpeedBaseValue; // 일꾼의 기본 속력
        public float moveSpeedIncreaseValue; // 강화시 기본 속력에 더해지는 증감 속력

        public float healthBaseValue; // 일꾼의 기본 기력
        public float healthIncreaseValue; // 강화시 기본 기력에 더해지는 증감 기력
        
        public float farmingSkillBaseValue; // 일꾼의 기본 농사스킬
        public float farmingSkillIncreaseValue; // 강화시 기본 농사스킬에 더해지는 증감 농사스킬
        
        public float adventureSkillBaseValue; // 일꾼의 기본 탐험 스킬
        public float adventureSkillIncreaseValue; // 강화시 기본 탐험스킬에 더해지는 증감 탐험 스킬
    }

	public partial class FarmerStatTable : DataTable<FarmerStatTableRow> { }
}