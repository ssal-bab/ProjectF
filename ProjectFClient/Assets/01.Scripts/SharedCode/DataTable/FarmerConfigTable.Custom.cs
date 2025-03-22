namespace ProjectF.DataTables
{
    public partial class FarmerConfigTableRow : ConfigTableRow<float>
    {
    }

    public partial class FarmerConfigTable : ConfigTable<FarmerConfigTableRow, float> 
    { 
        public float IdleDurationMin() => GetValue("IdleDurationMin");
        public float IdleDurationMax() => GetValue("IdleDurationMax");
        public float LevelSalesMultiplierValue() => GetValue("LevelSalesMultiplierValue");
        public float GradeSalesMultiplierValue() => GetValue("GradeSalesMultiplierValue");
    }
}
