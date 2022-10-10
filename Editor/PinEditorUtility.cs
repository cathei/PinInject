// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Cathei.PinInject.Editor
{
    public static class PinEditorUtility
    {
        [MenuItem("GameObject/PinInject/Scene Composition Root", false, 10)]
        public static void CreateSceneInjectRoot(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("SceneCompositionRoot");
            go.AddComponent<SceneCompositionRoot>();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("Assets/Create/PinInject/Persistent Composition Root", false, 100)]
        public static void CreateSharedInjectRoot()
        {
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(assetPath))
                assetPath = "Assets";

            if (!Directory.Exists(assetPath))
                assetPath = Path.GetDirectoryName(assetPath);

            assetPath = Path.Combine(assetPath, "PersistentCompositionRoot.prefab");
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            GameObject go = new GameObject("PersistentCompositionRoot");
            go.AddComponent<PersistentCompositionRoot>();

            var asset = PrefabUtility.SaveAsPrefabAsset(go, assetPath);

            UnityEngine.Object.DestroyImmediate(go);

            Undo.RegisterCreatedObjectUndo(asset, "Create " + asset.name);

            Selection.activeObject = asset;
        }
    }
}
