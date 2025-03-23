using H00N.DataTables;

namespace ProjectF.DataTables
{
    public class StatTableData
    {
        public float baseValue;
        public float increaseValue;
        public int levelStart;
        public int levelStep;

        public StatTableData(float[] stat)
        {
            baseValue = stat[0];
            increaseValue = stat[1];
            levelStart = (int)stat[2];
            levelStep = (int)stat[3];
        }
    }

	public partial class FarmerStatTableRow : DataTableRow
	{
        public StatTableData moveSpeed;
        public StatTableData health;
        public StatTableData farmingSkill;
        public StatTableData adventureSkill;
    }

	public partial class FarmerStatTable : DataTable<FarmerStatTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            foreach(FarmerStatTableRow tableRow in this)
            {
                tableRow.moveSpeed = new StatTableData(tableRow.moveSpeedStat);
                tableRow.health = new StatTableData(tableRow.healthStat);
                tableRow.farmingSkill = new StatTableData(tableRow.farmingSkillStat);
                tableRow.adventureSkill = new StatTableData(tableRow.adventureSkillStat);
            }
        }
    }
}