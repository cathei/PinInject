// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        internal static ContextConfiguration sceneContextConfig = null;
        
        /// <summary>
        /// Load and inject config a Scene.
        /// </summary>
        public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode, ContextConfiguration config = null)
        {
            sceneContextConfig = config;
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }

        /// <summary>
        /// Load and inject config a Scene.
        /// </summary>
        /// <remarks>
        /// Note: Calling this method in parallel may result in improper injection of the Configuration.
        /// </remarks>
        public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, ContextConfiguration config = null)
        {
            sceneContextConfig = config;
            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }
    }
}