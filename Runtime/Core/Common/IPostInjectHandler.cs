// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

namespace Cathei.PinInject
{
    /// <summary>
    /// Object that will receive PostInject event when injection has happened.
    /// </summary>
    public interface IPostInjectHandler
    {
        /// <summary>
        /// Event that will be called after injection.
        /// All dependencies for this object are injected and resolved by this timing.
        /// </summary>
        void PostInject();
    }
}