using System;
using Cysharp.Threading.Tasks;
using H00N.Resources;
using ProjectF.Farms;
using ProjectF.UI.Farms;
using UnityEngine;

namespace ProjectF.Tests
{
    public class TUI : MonoBehaviour
    {
        [SerializeField] float upPosition = 700f;
        [SerializeField] GameObject upButton = null;
        [SerializeField] GameObject downButton = null;

        private void Awake()
        {
            SlideDown();
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.LeftControl) == false)
                return;

            if(Input.GetKeyDown(KeyCode.E))
            {
                GameInstance.MainUser.nestData.level = 2;
                GameInstance.MainUser.nestData.hatchingEggList.Add(new Datas.EggHatchingData() {
                    eggID = 0,
                    hatchingStartTime = GameInstance.ServerTime
                });
                
                NestUI ui = FindObjectOfType<NestUI>();
                NestUICallbackContainer hi = null;
                hi = new NestUICallbackContainer(
                    async index => {
                        await UniTask.Delay(100);
                        GameInstance.MainUser.nestData.hatchingEggList.RemoveAt(index);
                        ui.Initialize(GameInstance.MainUser.nestData, hi);
                        return 1;
                    },
                    id => true,
                    id => true,
                    id => true,
                    id =>
                    {
                        Debug.Log($"Upgrade Nest!! id : {id}");
                        ui.Initialize(GameInstance.MainUser.nestData, hi);
                    }
                );

                ui.Initialize(GameInstance.MainUser.nestData, hi);
            }
        }

        public void SlideUp()
        {
            (transform as RectTransform).anchoredPosition = new Vector3(0, upPosition);
            upButton.SetActive(false);
            downButton.SetActive(true);
        }

        public void SlideDown()
        {
            (transform as RectTransform).anchoredPosition = new Vector3(0, 0f);
            downButton.SetActive(false);
            upButton.SetActive(true);
        }

        public void AddEggCrop()
        {
            CropSO cropData = ResourceManager.LoadResource<CropSO>("CropData_16");
            FindObjectOfType<Farm>().CropQueue.EnqueueCropData(cropData);
        }

        public void AddEgg()
        {
            CropSO cropData = ResourceManager.LoadResource<CropSO>("CropData_17");
            FindObjectOfType<Farm>().CropQueue.EnqueueCropData(cropData);
        }
    }
}
