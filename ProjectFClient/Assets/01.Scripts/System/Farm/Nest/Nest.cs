using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms
{
    public class Nest : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer = null;

        // [SerializeField] Sprite[] eggSpriteList = new Sprite[9]; // 우선은 9개 까지만

        public void Initialize()
        {
            UserNestData nestData = GameInstance.MainUser.nestData;
            nestData.OnLevelChangedEvent += UpdateVisual;
            UpdateVisual(nestData.level);
        }

        private void UpdateVisual(int level)
        {
            NestLevelTableRow tableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(level);
            new SetSprite(spriteRenderer, ResourceUtility.GetNestIconKey(tableRow.id));
        }
    }
}
