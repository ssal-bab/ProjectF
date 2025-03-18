namespace ProjectF.Datas
{
    public struct UserFieldGroupDataChecker
    {
        public UserFieldGroupDataChecker(UserData userData)
        {
            userData.fieldGroupData ??= new UserFieldGroupData();

            userData.fieldGroupData.fieldGroupDatas ??= new Dictionary<int, FieldGroupData>();
            CheckFieldGroupData(userData);
        }

        private void CheckFieldGroupData(UserData userData)
        {
            for(int i = 0; i < 6; ++i)
            {
                if(userData.fieldGroupData.fieldGroupDatas.TryGetValue(i, out FieldGroupData fieldGroupData) == false)
                {
                    fieldGroupData = new FieldGroupData() {
                        fieldGroupID = i,
                        // 0렙으로 시작. 튜토리얼 단계에서 건설하는 것 부터 시작이다.
                        // level = 0;

                        // 아직은 튜토리얼이 없으니 1렙부터 시작하도록 해두자.
                        level = 1,
                        fieldDatas = new Dictionary<int, FieldData>()
                    };

                    userData.fieldGroupData.fieldGroupDatas.Add(i, fieldGroupData);
                }

                for (int j = 0; j < 4; ++j)
                {
                    if (fieldGroupData.fieldDatas.ContainsKey(j))
                        continue;

                    FieldData fieldData = new FieldData() {
                        fieldID = j,
                        fieldState = EFieldState.Fallow,
                        currentCropID = -1,
                        currentGrowth = 0
                    };

                    fieldGroupData.fieldDatas.Add(j, fieldData);
                }
            }
        }
    }
}