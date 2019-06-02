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
    public abstract class PrefabSettingSO<T> : GenericSettingSO<UnityEngine.Object> where T : UnityEngine.Object
    {

        private UnityEngine.Object m_instance;
       
        internal UnityEngine.Object Instance
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
                    FireOnSetEvent();
                }
            }
        }

        public override string TestName
        {
            get
            {
                return "Validate prefab setup : " + SettingName;
            }
        }

        /**
        private bool IsPrefabSetting
        {
            get { return (SuggestedValue as UnityEngine.Object) != null 
                    && PrefabUtility.IsPartOfAnyPrefab(SuggestedValue as UnityEngine.Object); }
        }
    */

        private void FireOnSetEvent()
        {
            if (OnSetEvent != null)
            {
                OnSetEvent.Raise(GetFirstInstanceInScene());
            }
        }

        public override void Fix()
        {
            throw new Exception("The suggested value is a prefab, you need to override the Fix method in your *SettingSO to fix the failed test.");
        }

        private void InstantiatePrefab()
        {
            Instance = PrefabUtility.InstantiatePrefab(SuggestedValue);
            FireOnSetEvent();
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
                    if (candidate is Component)
                    {
                        go = ((Component)candidate).gameObject;
                    } else
                    {
                        go = (GameObject)candidate;
                    }
                    break;
                }
            }

            return go;
        }

        internal override ValidationResult ValidateSetting(Type validationTest)
        {
            ValidationResult result = null;
            UnityEngine.Object value;

            try
            {
                value = ActualValue;
            }
            catch (Exception e)
            {
                result = GetErrorResult(TestName, e.Message, validationTest.Name);
                result.RemoveCallbacks();
                return result;
            }

            GameObject sceneGO = GetFirstInstanceInScene();

            ResolutionCallback callback = new ResolutionCallback(InstantiatePrefab);
            if (AddToScene)
            {
                if (sceneGO == null)
                {
                    result = GetErrorResult(TestName, "The object required doesn't currently exist in the scene", validationTest.Name);
                    List<ResolutionCallback> callbacks = new List<ResolutionCallback>();
                    callbacks.Add(callback);
                    result.Callbacks = callbacks;
                    return result;
                }
            }

            GameObject suggestedGO;
            if (SuggestedValue is Component)
            {
                suggestedGO = (SuggestedValue as Component).gameObject;
            }
            else
            {
                suggestedGO = SuggestedValue as GameObject;
            }

            if (suggestedGO as UnityEngine.Object == null || !ReferenceEquals(suggestedGO, PrefabUtility.GetCorrespondingObjectFromSource(sceneGO)))
            {
                result = GetWarningResult(TestName, "There is no object in the scene that was instantiated from the suggested prefab.\n"
                    + "Suggested value = " + suggestedGO + "\n"
                    + "Actual Value = " + sceneGO + "\n"
                    + "This may be OK, in which case click the ignore button.", validationTest.Name);
                result.RemoveCallbacks();
                return result;
            }

            return GetPassResult(TestName, validationTest.Name);
        }
    }
}
