using H00N.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF
{
    public struct SetSprite
    {
        public SetSprite(Image image, string resourceKey)
        {
            SetSpriteAsync(image, resourceKey);
        }

        public SetSprite(SpriteRenderer spriteRenderer, string resourceKey)
        {
            SetSpriteAsync(spriteRenderer, resourceKey);
        }

        private async void SetSpriteAsync(Image image, string resourceKey)
        {
            Sprite resource = await ResourceManager.LoadResourceAsync<Sprite>(resourceKey);

            // Resource를 로딩하는 중에 파괴되었을 수 있다.
            if(image == null)
                return;

            image.gameObject.SetActive(resource != null);
            image.sprite = resource;
        }

        private async void SetSpriteAsync(SpriteRenderer spriteRenderer, string resourceKey)
        {
            Sprite resource = await ResourceManager.LoadResourceAsync<Sprite>(resourceKey);

            // Resource를 로딩하는 중에 파괴되었을 수 있다.
            if(spriteRenderer == null)
                return;

            spriteRenderer.gameObject.SetActive(resource != null);
            spriteRenderer.sprite = resource;
        }
    }
}