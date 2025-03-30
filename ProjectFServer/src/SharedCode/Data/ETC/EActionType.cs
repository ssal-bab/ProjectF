namespace ProjectF.Datas
{
    public enum EActionType
    {
        None,
        PlantSeed,          // n회 심기 or 특정 작물 n회 심기
        HarvestCrop,        // n회 수확하기 or 특정 작물 n회 수확하기
        AdventureComplete,  // n회 탐험 완료 or 특정 지역 n회 탐험 완료
        HatchEgg,           // n회 알 부화하기 or 특정 알 n회 부화하기
        OwnCrop,            // 특정 작물 n개 보유하기
    }
}