using System;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct CalculateFarmerLevelupGold
    {
        public int value;

        public CalculateFarmerLevelupGold(FarmerLevelupGoldTable table, ERarity rarity, int level)
        {
            var group = table.GetLevelupGoldGroup(rarity);
            float price = group.baseValue * group.multiplierValue * level;
            value = Convert.ToInt32(MathF.Floor(price));
        }
    }
}