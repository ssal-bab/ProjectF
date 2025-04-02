using System.Collections;
using System.Collections.Generic;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.UI.Adventure;
using UnityEngine;

namespace ProjectF.UI.Adventure
{
    public class TAdventureController : MonoBehaviour
    {
        [SerializeField] private AddressableAsset<AdventurePopupUI> adpopup;
        [SerializeField] private AddressableAsset<AdventureProgressInfoPanel> adprogresspanel;

        public async void OpenUI()
        {
            await adpopup.InitializeAsync();
            await adprogresspanel.InitializeAsync();

            AdventureTable table = DataTableManager.GetTable<AdventureTable>();
            int id = table.GetRow(0).id;

            // 임시 생성성
            AdventureData data = new AdventureData();

            if (!GameInstance.MainUser.adventureData.inAdventureAreaList.ContainsKey(id))
            {
                var ui = PoolManager.Spawn<AdventurePopupUI>(adpopup, GameDefine.MainPopupFrame);
                ui.StretchRect();
                ui.Initialize(data);
            }
            else
            {
                var ui = PoolManager.Spawn<AdventureProgressInfoPanel>(adprogresspanel, GameDefine.MainPopupFrame);
                ui.StretchRect();
                ui.Initialize(data);
            }

        }
    }
}
