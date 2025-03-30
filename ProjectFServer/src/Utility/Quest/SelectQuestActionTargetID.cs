using System;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct SelectQuestActionTargetID
    {
        public int actionTargetID;

        public SelectQuestActionTargetID(EActionType actionType, UserData userData)
        {
            actionTargetID = -1;
            switch(actionType) 
            {
                case EActionType.OwnTargetCrop:
                case EActionType.PlantTargetSeed:
                case EActionType.HarvestTargetCrop:
                    actionTargetID = SelectCropID(userData);
                    break;
                case EActionType.HatchTargetEgg:
                    actionTargetID = SelectEggID(userData);
                    break;
            }
        }

        private int SelectCropID(UserData userData)
        {
            // 나중엔 적잘한 알고리즘을 통해 하나를 골라야 한다.
            // 아직은 기반이 없으니 가장 낮은 작물 5개중 하나를 랜덤으로 뽑자.
            Random random = new Random();
            while(true)
            {
                int randomID = random.Next(0, 5);
                CropTableRow tableRow = DataTableManager.GetTable<CropTable>().GetRow(randomID);
                if(tableRow != null)
                    return randomID;
            }
        }

        private int SelectEggID(UserData userData)
        {
            // 나중엔 적잘한 알고리즘을 통해 하나를 골라야 한다.
            // 아직은 기반이 없으니 가장 낮은 알 3개중 하나를 랜덤으로 뽑자.
            Random random = new Random();
            while(true)
            {
                int randomID = random.Next(0, 3);
                EggTableRow tableRow = DataTableManager.GetTable<EggTable>().GetRow(randomID);
                if(tableRow != null)
                    return randomID;
            }
        }
    }
}