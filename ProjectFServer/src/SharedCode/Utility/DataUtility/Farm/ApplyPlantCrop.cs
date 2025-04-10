namespace ProjectF.Datas
{
    public struct ApplyPlantCrop
    {
        public FieldData fieldData;

        public ApplyPlantCrop(UserFieldGroupData userFieldGroupData, int fieldGroupID, int fieldID, int cropID)
        {
            fieldData = new GetFieldData(userFieldGroupData, fieldGroupID, fieldID).fieldData;
            if(fieldData == null)
                return;

            fieldData.currentCropID = cropID;
            fieldData.currentGrowth = 0;
            fieldData.fieldState = EFieldState.Dried;
        }
    }
}