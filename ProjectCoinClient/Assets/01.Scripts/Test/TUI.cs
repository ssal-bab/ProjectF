using H00N.Resources;
using ProjectCoin.Farms;
using UnityEngine;

namespace ProjectCoin.Tests
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
            FindObjectOfType<Farm>().EnqueueCropData(cropData);
        }

        public void AddEgg()
        {
            CropSO cropData = ResourceManager.LoadResource<CropSO>("CropData_17");
            FindObjectOfType<Farm>().EnqueueCropData(cropData);
        }
    }
}
