using ProjectCoin.Datas;
using UnityEngine;

namespace ProjectCoin.Farms
{
    public class FieldVisual : MonoBehaviour
    {
        [SerializeField] SpriteRenderer plantRenderer = null;
        [SerializeField] GameObject wetEffectObject = null;

        private Field field = null;

        private void Awake()
        {
            field = GetComponent<Field>();
        }

        private void Start()
        {
            plantRenderer.sprite = null;
            wetEffectObject.SetActive(false);

            field.OnStateChangedEvent += HandleStateChanged;
            field.OnGrowUpEvent += HandleGrowUp;
        }

        private void HandleStateChanged(EFieldState state)
        {
            Debug.Log($"Field state changed : {state}");

            bool wetObjectActive = state == EFieldState.Growing;
            if(wetEffectObject.activeSelf == wetObjectActive)
                return;
            
            wetEffectObject.SetActive(wetObjectActive);
        }

        private void HandleGrowUp(int growStep)
        {
            plantRenderer.sprite = field.CurrentCropData.cropPlantSprites[growStep];
        }
    }
}
