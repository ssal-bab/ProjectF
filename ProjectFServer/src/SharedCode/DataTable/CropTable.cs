using System;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    [Serializable]
    public class CropTableRow : DataTableRow
    {
        public ECropType cropType;
        public int seedItemID;
        public int growthStep;
        public int growthRate;
        public int productCropID;
        public string nameLocalKey;
    }

    public class CropTable : DataTable<CropTableRow> { }
}