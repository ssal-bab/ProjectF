namespace ProjectF.Datas
{
    public struct GetFieldData
    {
        public FieldData fieldData;
        
        public GetFieldData(UserFieldGroupData userFieldGroupData, int fieldGroupID, int fieldID)
        {
            fieldData = null;
            if(userFieldGroupData.fieldGroupDatas.TryGetValue(fieldGroupID, out FieldGroupData fieldGroupData) == false)
                return;

            fieldGroupData.fieldDatas.TryGetValue(fieldID, out fieldData);
        }
    }
}