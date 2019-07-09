using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WizardsCode.digitalpainting;
using WizardsCode.validation;

namespace WizardsCode.extension
{

    public static class AssetDatabaseUtility
    {
        internal static string dataFolderName = "Digital Painting Data";
        internal static string defaultManagerProfileName = "DigitalPaintingManagerProfile_Default.asset";

        /// <summary>
        /// Get all the assets of a given type in an Asset folder.
        /// </summary>
        /// <typeparam name="T">The type of Asset we are interested in.</typeparam>
        /// <param name="path">The path to the asset folder (not including the leading `Assets/`.</param>
        /// <returns>An array of assets from the folder.</returns>
        public static T[] GetAssetsList<T>(string path)
        {
            ArrayList arrayList = new ArrayList();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

            foreach (string fileName in fileEntries)
            {
                string temp = fileName.Replace("\\", "/");
                int index = temp.LastIndexOf("/");
                string localPath = "Assets/" + path;

                if (index > 0)
                {
                    localPath += temp.Substring(index);
                }

                Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

                if (t != null)
                {
                    arrayList.Add(t);
                }
            }

            T[] result = new T[arrayList.Count];

            for (int i = 0; i < arrayList.Count; i++)
            {
                result[i] = (T)arrayList[i];
            }

            return result;
        }

        private static string GetPathToDefaultDataCollection()
        {
            return "DigitalPainting/Assets/Digital Painting/Data/Default Collection";
        }

        /// <summary>
        /// Copy all the Setting SOs in the default data directory to a new location, renaming them as appropriate.
        /// Configure the Digital Painting Manager appropriately.
        /// This is used to create initial setting SOs for a new Digital Painting scene.
        /// </summary>
        /// <param name="toPath">The path to copy the assets to (not including the leading `Assets/`.</param>
        /// <param name="suffix">The suffix to use to make the Setting SO asset names unique.</param>
        public static DigitalPaintingManagerProfile SetupDefaultSettings(string toPath, string suffix)
        {
            string fromPath = GetPathToDefaultDataCollection();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + fromPath);
            string fromProfilePath = "Assets/" + fromPath + "/" + defaultManagerProfileName;
            string toProfilePath = toPath + "/" + dataFolderName + "/" + defaultManagerProfileName.Replace("_Default", "_" + suffix);
            AssetDatabase.CopyAsset(fromProfilePath, toProfilePath);

            DigitalPaintingManagerProfile profile = AssetDatabase.LoadAssetAtPath<DigitalPaintingManagerProfile>(toProfilePath);

            foreach (string fileName in fileEntries)
            {
                string temp = fileName.Replace("\\", "/");
                int index = temp.LastIndexOf("/");
                string localPath = "Assets/" + fromPath;

                if (index > 0)
                {
                    localPath += temp.Substring(index);
                }

                Object original = AssetDatabase.LoadAssetAtPath(localPath, typeof(AbstractSettingSO));

                if (original != null)
                {
                    string filename = Path.GetFileName(localPath);
                    filename = filename.Replace("_Default", "_" + suffix);
                    string fullToPath = toPath + "/" + dataFolderName + "/" + filename;
                    AssetDatabase.CopyAsset(localPath, fullToPath);

                    object newAsset = AssetDatabase.LoadAssetAtPath(fullToPath, typeof(AbstractSettingSO));

                    // TODO: Use reflection to setup the profile
                    if (filename.IndexOf("Camera") != -1)
                    {
                        profile.CameraSettings = (CameraSettingSO)newAsset;
                    }
                    else if (filename.IndexOf("ColorSpace") != -1)
                    {
                        profile.ColorSpaceSettings = (ColorSpaceSettingSO)newAsset;
                    }
                    else if (filename.IndexOf("Terrain") != -1)
                    {
                        profile.TerrainSettings = (PrefabSettingSO)newAsset;
                    }
                    else if (filename.IndexOf("ScreenSpaceShadow") != -1)
                    {
                        profile.ScreenSpaceSettings = (ScreenSpaceShadowsSettingSO)newAsset;
                    }
                    else if (filename.IndexOf("Agent") != -1)
                    {
                        profile.AgentSettings = (AgentSettingSO)newAsset;
                    }
                }
            }

            return profile;
        }
    }
}
