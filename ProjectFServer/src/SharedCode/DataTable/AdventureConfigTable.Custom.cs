using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class AdventureConfigTableRow : ConfigTableRow<float>
    {
    }

    public partial class AdventureConfigTable : ConfigTable<AdventureConfigTableRow, float> 
    {
        public float LootItem1Probability => GetValue("LootItem1Probability");
        public float LootItem2Probability => GetValue("LootItem2Probability");
        public float LootItem3Probability => GetValue("LootItem3Probability");
        public float LootItem4Probability => GetValue("LootItem4Probability");

        public float LootSeed1Probability => GetValue("LootSeed1Probability");
        public float LootSeed2Probability => GetValue("LootSeed2Probability");
        public float LootSeed3Probability => GetValue("LootSeed3Probability");
    }
}