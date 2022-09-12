// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Globalization;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Linq;

namespace Cathei.PinInject.Editor
{
    [CustomPropertyDrawer(typeof(InjectNameSelectionAttribute))]
    public class InjectNameSelectionPropertyDrawer : PropertyDrawer
    {
        private List<IInjectContext> _tempContexts = new List<IInjectContext>();
        private List<DisplayContent> _tempContents = new List<DisplayContent>();

        private struct DisplayContent
        {
            public GUIContent content;
            public string value;
        }

        private SceneInjectRoot FindSceneInjectRoot(Scene scene)
        {
            if (!scene.IsValid())
                return null;

            var roots = UnityEngine.Object.FindObjectsOfType<SceneInjectRoot>(false);

            foreach (var root in roots)
            {
                if (root.gameObject.scene == scene)
                    return root;
            }

            return null;
        }

        private void AddConstStrings(Type type, List<DisplayContent> list)
        {
            string typeName = type.Name;

            var fieldInfos = type.GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var fieldInfo in fieldInfos)
            {
                if (!fieldInfo.IsLiteral || fieldInfo.FieldType != typeof(string))
                    continue;

                string value = fieldInfo.GetValue(null) as string;

                list.Add(new DisplayContent
                {
                    content = new GUIContent($"{typeName}/{value}"),
                    value = value
                });
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var component = property.serializedObject.targetObject as Component;

            if (component == null)
                return;

            var sceneRoot = FindSceneInjectRoot(component.gameObject.scene);
            var sharedRoot = sceneRoot != null ? sceneRoot.sharedRoot : null;

            _tempContents.Clear();

            if (sharedRoot != null)
            {
                sharedRoot.GetComponents(_tempContexts);

                foreach (var context in _tempContexts)
                    AddConstStrings(context.GetType(), _tempContents);
            }

            if (sceneRoot != null)
            {
                sceneRoot.GetComponents(_tempContexts);

                foreach (var context in _tempContexts)
                    AddConstStrings(context.GetType(), _tempContents);
            }

            component.GetComponentsInParent(true, _tempContexts);

            for (int i = _tempContexts.Count - 1; i >= 0 ; i--)
                AddConstStrings(_tempContexts[i].GetType(), _tempContents);

            EditorGUI.BeginChangeCheck();

            int selectedIndex = _tempContents.FindIndex(x => x.value == property.stringValue);

            selectedIndex = EditorGUI.Popup(
                position, label, selectedIndex, _tempContents.Select(x => x.content).ToArray());

            if (EditorGUI.EndChangeCheck() && selectedIndex >= 0)
            {
                property.stringValue = _tempContents[selectedIndex].value;
            }
        }
    }
}
