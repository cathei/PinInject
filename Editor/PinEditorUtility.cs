using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEditor;

namespace Cathei.PinInject.Editor
{
    public static class PinEditorUtility
    {
        [MenuItem("GameObject/PinInject/Scene Inject Root", false, 10)]
        public static void CreateSceneInjectRoot(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("SceneInjectRoot");
            go.AddComponent<SceneInjectRoot>();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}
