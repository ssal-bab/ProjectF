namespace ProjectF.Datas
{
    [System.Serializable]
    public class FieldData
    {
        public int fieldID = 0;

        public EFieldState fieldState = EFieldState.None;
        public int currentCropID = 0;
        public int currentGrowthStep = 0;
    }
}