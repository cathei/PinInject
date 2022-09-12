// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    [RequireComponent(typeof(Text))]
    public class UITextBinder : UIBinderBase<Text, string>
    {
        protected override void HandleEvent(string text)
        {
            target.text = text;
        }
    }
}
