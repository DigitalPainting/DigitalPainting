using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using WizardsCode.Editor;
using WizardsCode.Extension;
using WizardsCode.Plugin;
using WizardsCode.Utility;

namespace WizardsCode.Validation
{
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_PrefabSettingSO", menuName = "Wizards Code/Validation/Prefab")]
    public class PrefabSettingSO : AbstractSettingSO<UnityEngine.Object>
    {
        [Tooltip("If the suggested value is a prefab should a copy of the object be added to the scene.")]
        public bool AddToScene = true;
        [Tooltip("The spawn location for the prefab.")]
        public Vector3 spawnPosition;
    
        private GameObject m_instance;
       
        internal GameObject Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = GetFirstInstanceInScene();
                }
                return m_instance;
            }
            set
            {
                if (!UnityEngine.Object.ReferenceEquals(m_instance, value))
                {
                    m_instance = value;
                }
            }
        }

        protected override UnityEngine.Object ActualValue
        { 
            get { return Instance; }
            set { Instance = (GameObject)value; }
        }
        
        public override void Fix()
        {
            throw new NotImplementedException();
        }

        public override string TestName
        {
            get
            {
                return "Check prefab setup defined in " + name + ":" + SettingName;
            }
        }

        internal virtual void InstantiatePrefab()
        {
            Instance = ConvertToGameObject(PrefabUtility.InstantiatePrefab(SuggestedValue));
            Instance.transform.position = spawnPosition;
        }

        /// <summary>
        /// Get an instance object that was created by the Suggested Value (assuming it is a Prefab).
        /// </summary>
        /// <returns></returns>
        internal GameObject GetFirstInstanceInScene()
        {
            GameObject go = null;
            UnityEngine.Object[] gos = GameObject.FindObjectsOfType(SuggestedValue.GetType());
            foreach (UnityEngine.Object candidate in gos)
            {
                if (UnityEngine.Object.ReferenceEquals(PrefabUtility.GetCorrespondingObjectFromOriginalSource(candidate), SuggestedValue))
                {
                    go = ConvertToGameObject(candidate);
                    break;
                }
            }

            return go;
        }

        internal override ValidationResult ValidateSetting(Type validationTest, AbstractPluginManager pluginManager)
        {
            ValidationResult result = null;
            
            if (Instance == null)
            {
                if (AddToScene)
                {
                    result = GetErrorResult(TestName, pluginManager, SuggestedValue.name + " is required but doesn't currently exist in the scene", validationTest.Name);
                } else
                {
                    result = GetWarningResult(TestName, pluginManager, SuggestedValue.name + " doesn't currently exist in the scene. It is not required, if you don't want it then click the ignore button.", validationTest.Name);
                }
                List<ResolutionCallback> callbacks = new List<ResolutionCallback>();
                ResolutionCallback callback = new ResolutionCallback(InstantiatePrefab);
                callbacks.Add(callback);
                result.Callbacks = callbacks;
                return result;
            }

            return GetPassResult(TestName, pluginManager, validationTest.Name);
        }
    }
}
