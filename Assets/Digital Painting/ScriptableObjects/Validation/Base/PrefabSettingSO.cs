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
using wizardscode.extension;
using wizardscode.utility;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "PrefabSettingSO", menuName = "Wizards Code/Validation/Prefab")]
    public class PrefabSettingSO : AbstractSettingSO<UnityEngine.Object>
    {
        [Tooltip("If the suggested value is a prefab should a copy of the object be added to the scene.")]
        public bool AddToScene = false;

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
        
        public override void Fix()
        {
            throw new NotImplementedException();
        }

        public override string TestName
        {
            get
            {
                return "Validate prefab setup : " + SettingName;
            }
        }

        private void InstantiatePrefab()
        {
            Instance = ConvertToGameObject(PrefabUtility.InstantiatePrefab(SuggestedValue));
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

        /// <summary>
        /// If the candidate object is a Component return the GameObject it is attached to.
        /// If the candidate is already a Game Object return it.
        /// </summary>
        /// <param name="candidate">The object that is either a component or a GameObject</param>
        /// <returns></returns>
        private static GameObject ConvertToGameObject(UnityEngine.Object candidate)
        {
            GameObject go;
            if (candidate is Component)
            {
                go = ((Component)candidate).gameObject;
            }
            else
            {
                go = (GameObject)candidate;
            }

            return go;
        }

        internal override ValidationResult ValidateSetting(Type validationTest)
        {
            ValidationResult result = null;

            if (AddToScene && Instance == null)
            {
                result = GetErrorResult(TestName, "The object required doesn't currently exist in the scene", validationTest.Name);
                List<ResolutionCallback> callbacks = new List<ResolutionCallback>();
                ResolutionCallback callback = new ResolutionCallback(InstantiatePrefab);
                callbacks.Add(callback);
                result.Callbacks = callbacks;
                return result;
            }

            return GetPassResult(TestName, validationTest.Name);
        }
    }
}
