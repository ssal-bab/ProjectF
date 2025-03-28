namespace ProjectF.Datas
{
    public class FieldData
    {
        public int fieldID = 0;

        public EFieldState fieldState = EFieldState.None;
        public int currentCropID = 0;
        public int currentGrowth = 0;

        public void UpdateData(FieldData data)
        {
            fieldID = data.fieldID;
            fieldState = data.fieldState;
            currentCropID = data.currentCropID;
            currentGrowth = data.currentGrowth;
        }
    }
}