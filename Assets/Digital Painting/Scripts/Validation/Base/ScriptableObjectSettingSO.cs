using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using wizardscode.extension;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_ScriptableObjectSettingSO", menuName = "Wizards Code/Validation/Scriptable Object")]
    public class ScriptableObjectSettingSO : AbstractSettingSO<MonoScript>
    {
        private ScriptableObject m_instance;
       
        internal ScriptableObject Instance
        {
            get
            {
                if (m_instance == null)
                {
                    UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetPath);
                    if (assets.Length > 0)
                    {
                        m_instance = (ScriptableObject)assets[0];
                    }
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }

        private string AssetPath
        {
            get {
                Scene scene = EditorSceneManager.GetActiveScene();
                string sceneName = scene.name;
                string scenePath = scene.path.Substring(0, scene.path.Length - ("/" + sceneName + ".unity").Length);
                string folder = scenePath + "/" + AssetDatabaseUtility.dataFolderName;
                return folder + "/" + SettingName + ".asset";
            }
        }

        protected override MonoScript ActualValue
        {
            get
            {
                if (Instance != null)
                {
                    return MonoScript.FromScriptableObject((ScriptableObject)Instance);
                }
                else
                {
                    return null;
                }
            }
            set { Instance = ScriptableObject.CreateInstance(value.GetType()); }
        }
        
        public override void Fix()
        {
            throw new NotImplementedException();
        }

        public override string TestName
        {
            get
            {
                return "Check Scriptable Object setup defined in " + name + ":" + SettingName;
            }
        }

        internal virtual void InstantiateScriptableObject()
        {
            Instance = ScriptableObject.CreateInstance(SuggestedValue.GetClass());
            AssetDatabase.CreateAsset(Instance, AssetPath);
            AssetDatabase.SaveAssets();
        }

        internal override ValidationResult ValidateSetting(Type validationTest)
        {
            ValidationResult result = null;
            
            if (Instance == null)
            {
                result = GetErrorResult(TestName, SuggestedValue.name + " is required but doesn't currently exist in the scene.", validationTest.Name, new ResolutionCallback(InstantiateScriptableObject));
                return result;
            }

            return GetPassResult(TestName, validationTest.Name);
        }
    }
}
