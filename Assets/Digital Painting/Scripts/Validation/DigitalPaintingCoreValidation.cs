using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using wizardscode.extension;
using wizardscode.validation;

namespace wizardscode.digitalpainting
{
    public class DigitalPaintingCoreValidation : ValidationTest<DigitalPaintingManager>
    {
        public override ValidationTest<DigitalPaintingManager> Instance => new DigitalPaintingCoreValidation();

        internal override string ProfileType { get { return "DigitalPaintingManagerProfile"; } }

        internal override bool InitialCustomValidations()
        {
            bool isPass = base.InitialCustomValidations();

            string path = GetPathToScene();
            if (AssetDatabase.IsValidFolder(path + "/" + AssetDatabaseUtility.dataFolderName))
            {
                AddOrUpdateAsPass("Data Directory Existence", "The Digital Painting Data exists.");
            }
            else
            {
                ResolutionCallback callback = new ResolutionCallback(new ProfileCallback(CreateDataDirectory));
                AddOrUpdateAsWarning("Data Directory Existence", "The Digital Painting Data folder does not exist.", callback);
                return false;
            }

            return isPass;
        }

        private void CreateDataDirectory()
        {
            string path = GetPathToScene();
            AssetDatabase.CreateFolder(path, AssetDatabaseUtility.dataFolderName);
            AssetDatabaseUtility.CopyDefaultSettingSOs(path, EditorSceneManager.GetActiveScene().name);
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
