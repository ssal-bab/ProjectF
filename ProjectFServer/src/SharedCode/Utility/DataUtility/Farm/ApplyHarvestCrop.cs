namespace ProjectF.Datas
{
    public struct ApplyHarvestCrop
    {
        public FieldData fieldData;

        public ApplyHarvestCrop(UserFieldGroupData userFieldGroupData, int fieldGroupID, int fieldID)
        {
            fieldData = new GetFieldData(userFieldGroupData, fieldGroupID, fieldID).fieldData;
            if(fieldData == null)
                return;

            fieldData.currentCropID = -1;
            fieldData.currentGrowth = 0;
            fieldData.fieldState = EFieldState.Fallow;
        }
    }
}