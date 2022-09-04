#if PIN_TEXTMESHPRO

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class UITextMeshProBinder : UIBinderBase<TMP_Text, string>
    {
        protected override void HandleEvent(string text)
        {
            target.text = text;
        }
    }
}

#endif