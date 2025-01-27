namespace ProjectF.Datas
{
    public struct UserFarmDataChecker
    {
        public UserFarmDataChecker(UserData userData)
        {
            userData.farmData ??= new UserFarmData();

            userData.farmData.fieldGroupDatas ??= new Dictionary<int, FieldGroupData>();
            CheckFieldGroupData(userData);
        }

        private void CheckFieldGroupData(UserData userData)
        {
            for(int i = 0; i < 6; ++i)
            {
                if(userData.farmData.fieldGroupDatas.TryGetValue(i, out FieldGroupData fieldGroupData) == false)
                {
                    fieldGroupData = new FieldGroupData() {
                        fieldGroupID = i,
                        fieldDatas = new Dictionary<int, FieldData>()
                    };

                    userData.farmData.fieldGroupDatas.Add(i, fieldGroupData);
                }

                for (int j = 0; j < 4; ++j)
                {
                    if (fieldGroupData.fieldDatas.TryGetValue(j, out FieldData fieldData) == false)
                    {
                        fieldData = new FieldData() {
                            fieldID = j,
                            fieldState = EFieldState.Fallow,
                            currentCropID = -1,
                            currentGrowthStep = 0
                        };

                        fieldGroupData.fieldDatas.Add(j, fieldData);
                    }
                }
            }
        }
    }
}