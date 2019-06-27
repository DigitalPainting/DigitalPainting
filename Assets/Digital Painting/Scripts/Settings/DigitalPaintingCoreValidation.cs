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

        internal override Type ProfileType
        {
            get
            {
                return typeof(DigitalPaintingManagerProfile);
            }
        }

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
                ResolutionCallback callback = new ResolutionCallback(new ProfileCallback(CreateDefaultSettingsData));
                AddOrUpdateAsWarning("Data Directory Existence", "The Digital Painting Data folder does not exist.", callback);
                return false;
            }

            return isPass;
        }

        private void CreateDefaultSettingsData()
        {
            string path = GetPathToScene();
            AssetDatabase.CreateFolder(path, AssetDatabaseUtility.dataFolderName);
            DigitalPaintingManagerProfile profile = AssetDatabaseUtility.SetupDefaultSettings(path, EditorSceneManager.GetActiveScene().name);
            DigitalPaintingManager manager = GameObject.FindObjectOfType<DigitalPaintingManager>();
            manager.m_pluginProfile = profile;
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
