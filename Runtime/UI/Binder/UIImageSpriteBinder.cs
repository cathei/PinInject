using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    [RequireComponent(typeof(Image))]
    public class UIImageSpriteBinder : UIBinderBase<Image, Sprite>
    {
        protected override void HandleEvent(Sprite sprite)
        {
            target.sprite = sprite;
        }
    }
}
