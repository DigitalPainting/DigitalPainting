using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WizardsCode.Extension
{
    public static class LayerMaskExtension
    {
        /// <summary>
        /// Create a layer at the next available index. Returns silently if layer already exists.
        /// Logs error if maximum number of layers reached.
        /// See https://forum.unity.com/threads/adding-layer-by-script.41970/
        /// </summary>
        /// <param name="name">Name of the layer to create</param>
        public static void CreateLayer(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentNullException("name", "New layer name string is either null or empty.");

            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layerProps = tagManager.FindProperty("layers");
            var propCount = layerProps.arraySize;

            SerializedProperty firstEmptyProp = null;

            for (var i = 0; i < propCount; i++)
            {
                var layerProp = layerProps.GetArrayElementAtIndex(i);
                var stringValue = layerProp.stringValue;

                if (stringValue == name) return;

                if (i < 8 || stringValue != string.Empty) continue;

                if (firstEmptyProp == null)
                    firstEmptyProp = layerProp;
            }

            if (firstEmptyProp == null)
            {
                Debug.LogError("Maximum limit of " + propCount + " layers exceeded. Layer \"" + name + "\" not created.");
                return;
            }

            firstEmptyProp.stringValue = name;
            tagManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// Create a layer at the specified index index. Returns silently if layer already exists.
        /// FIXME return an error if the layer index is already in use
        /// </summary>
        /// <param name="index">Index of the layer to create</param>
        /// <param name="name">Name of the layer to create</param>
        public static void CreateLayer(int index, string name)
        {
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layerProps = tagManager.FindProperty("layers");
            var layerProp = layerProps.GetArrayElementAtIndex(index);

            layerProp.stringValue = name;
            tagManager.ApplyModifiedProperties();
        }
    }
}
