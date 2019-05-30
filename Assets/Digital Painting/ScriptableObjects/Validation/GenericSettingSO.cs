using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using wizardscode.extension;
using wizardscode.utility;

namespace wizardscode.validation
{
    public abstract class GenericSettingSO<T> : AbstractSettingSO
    {
        /// <summary>
        /// A GenericSettingsSO defines a desired setting for a 
        /// single value. These are used in the validation system to find the optimal
        /// settings when different plugins demand different settings.
        /// </summary>
        [Tooltip("The suggested value for the setting. Other values may work, but if in doubt use this setting.")]
        public T SuggestedValue;

        protected abstract T ActualValue { get; set;  }

        public override ValidationResult Validate(Type validationTest)
        {
            ValidationResult result;
            if (!Nullable)
            { 
                string testName = "Suggested Value (" + validationTest.Name.BreakCamelCase() + ")";
                if (SuggestedValue is UnityEngine.Object)
                {
                    if (SuggestedValue as UnityEngine.Object == null)
                    {
                        result = GetErrorResult(testName, "Suggested value cannot be null.", validationTest.Name);
                        result.ReportingTest.Add(validationTest.Name);
                        return result;
                    } else
                    {
                        result = GetPassResult(testName, validationTest.Name);
                    }
                } else if (SuggestedValue == null)
                {
                    result = GetErrorResult(testName, "Suggested value cannot be null.", validationTest.Name);
                    result.ReportingTest.Add(validationTest.Name);
                    return result;
                }
                else
                {
                    result = GetPassResult(testName, validationTest.Name);
                }
            }
            return ValidateSetting(validationTest);
        }

        public override void Fix()
        {
            ActualValue = SuggestedValue;
        }

        private void InstantiatePrefab()
        {
            PrefabUtility.InstantiatePrefab(SuggestedValue as UnityEngine.Object);
        }

        /// <summary>
        /// Get an instance object that was created by the Suggested Value (assuming it is a Prefab).
        /// </summary>
        /// <returns></returns>
        private GameObject GetFirstInstanceInScene()
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
            string testName = "Generic Setting Test Suite";
            ValidationResult result = null;

            if (PrefabUtility.IsPartOfAnyPrefab(SuggestedValue as UnityEngine.Object)) {
                GameObject go = GetFirstInstanceInScene();


                ResolutionCallback callback = new ResolutionCallback(InstantiatePrefab);
                if (AddToScene) {
                    if (go == null)
                    {
                        result = GetErrorResult(testName, "The object required doesn't currently exist in the scene", validationTest.Name);
                        List<ResolutionCallback> callbacks = new List<ResolutionCallback>();
                        callbacks.Add(callback);
                        result.Callbacks = callbacks;
                        return result;
                    }
                } else
                {
                    result.Callbacks.Remove(callback);
                }

                if (PrefabUtility.IsPartOfAnyPrefab(SuggestedValue as UnityEngine.Object))
                {
                    if (ActualValue as UnityEngine.Object == null || !ReferenceEquals(ActualValue, go))
                    {
                        result = GetWarningResult(testName, "The value set is not an object in the scene that has been instantiated from the suggested value prefab. This may be OK, in which case click the ignore button.", validationTest.Name);
                        return result;
                    }
                }
            } else if (!object.Equals(ActualValue, SuggestedValue))
            {
                result = GetWarningResult(testName, "The value set is not the same as the suggested value. This may be OK, in which case click the ignore checkbox.", validationTest.Name);
                return result;
            }

            return GetPassResult(testName, validationTest.Name);
        }
    }
}
