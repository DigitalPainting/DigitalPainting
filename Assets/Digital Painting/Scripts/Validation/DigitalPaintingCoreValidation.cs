using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using wizardscode.validation;

namespace wizardscode.digitalpainting
{
    public class DigitalPaintingCoreValidation : ValidationTest<DigitalPaintingManager>
    {
        string dataFolderName = "Digital Painting Data";

        public override ValidationTest<DigitalPaintingManager> Instance => new DigitalPaintingCoreValidation();

        internal override string ProfileType { get { return "DigitalPaintingManagerProfile"; } }

        internal override bool InitialCustomValidations()
        {
            bool isPass = base.InitialCustomValidations();

            ValidationResult result;

            string path = GetPathToScene();
            if (AssetDatabase.IsValidFolder(path + "/" + dataFolderName))
            {
                AddOrUpdateAsPass("Data Directory Existence", "The Digital Painting Data exists.");
            }
            else
            {
                AddOrUpdateAsWarning("Data Directory Existence", "The Digital Painting Data does not exist.");
                isPass = false;
            }

            return isPass;
        }

        private void CopyDefaultSOCollection()
        {
            string path = GetPathToScene();
            AssetDatabase.CreateFolder(path, dataFolderName);

            AssetDatabase.CopyAsset("Assets/DigitalPainting/Assets/Digital Painting/Data/Default Collection/CameraSettingSO_Default.asset", path + "/" + dataFolderName + "/CameraSettingSO_Default.asset");
            AssetDatabase.SaveAssets();
        }

        private static string GetPathToScene()
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            string sceneName = scene.name;
            string path = scene.path.Substring(0, scene.path.Length - ("/" + sceneName + ".unity").Length);
            return path;
        }
    }
}
