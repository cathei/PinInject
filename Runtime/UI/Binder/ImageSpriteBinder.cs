// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    [RequireComponent(typeof(Image))]
    public class ImageSpriteBinder : UIBinderBase<Image, Sprite>
    {
        protected override void HandleEvent(Sprite sprite)
        {
            target.sprite = sprite;
        }
    }
}
