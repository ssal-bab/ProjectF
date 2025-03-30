namespace ProjectF.Datas
{
    public struct UserRepeatQuestDataChecker
    {
        public UserRepeatQuestDataChecker(UserData userData)
        {
            UserRepeatQuestData repeatQuestData = userData.repeatQuestData ??= new UserRepeatQuestData();
            
            repeatQuestData.cropRepeatQuestData ??= CreateNewRepeatQuestData();
            repeatQuestData.farmRepeatQuestData ??= CreateNewRepeatQuestData();
            repeatQuestData.adventureRepeatQuestData ??= CreateNewRepeatQuestData();
        }

        private RepeatQuestData CreateNewRepeatQuestData()
        {
            return new RepeatQuestData() {
                questID = 0,
                currentProgress = 0,
                repeatCount = 1
            };
        }
    }
}