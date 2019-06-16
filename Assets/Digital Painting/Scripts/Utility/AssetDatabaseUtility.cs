using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using wizardscode.validation;

namespace wizardscode.extension
{

    public static class AssetDatabaseUtility
    {
        internal static string dataFolderName = "Digital Painting Data";

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
        /// This is used to create initial setting SOs for a new Digital Painting scene.
        /// </summary>
        /// <param name="toPath">The path to copy the assets to (not including the leading `Assets/`.</param>
        /// <param name="suffix">The suffix to use to make the Setting SO asset names unique.</param>
        public static void CopyDefaultSettingSOs(string toPath, string suffix)
        {
            string fromPath = GetPathToDefaultDataCollection();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + fromPath);

            foreach (string fileName in fileEntries)
            {
                string temp = fileName.Replace("\\", "/");
                int index = temp.LastIndexOf("/");
                string localPath = "Assets/" + fromPath;

                if (index > 0)
                {
                    localPath += temp.Substring(index);
                }

                Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(AbstractSettingSO));

                if (t != null)
                {
                    string filename = Path.GetFileName(localPath);
                    filename = filename.Replace("_Default", "_" + suffix);
                    AssetDatabase.CopyAsset(localPath, toPath + "/" + dataFolderName + "/" + filename);
                }
            }
        }
    }
}
