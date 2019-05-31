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

        protected abstract T ActualValue { get; set; }

        protected virtual string TestName {
            get { return "Generic Setting Test Suite (replace this name by overriding the TestName getter in you *SettingSO)"; }
        }

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
            if (PrefabUtility.IsPartOfAnyPrefab(SuggestedValue as UnityEngine.Object))
            {
                throw new Exception("The suggested value is a prefab, you need to override the Fix method in your *SettingSO to fix the failed test.");
            }
            else
            {
                ActualValue = SuggestedValue;
            }
        }

        private void InstantiatePrefab()
        {
            PrefabUtility.InstantiatePrefab(SuggestedValue as UnityEngine.Object);
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

            if (PrefabUtility.IsPartOfAnyPrefab(SuggestedValue as UnityEngine.Object)) {
                GameObject go = GetFirstInstanceInScene();

                ResolutionCallback callback = new ResolutionCallback(InstantiatePrefab);
                if (AddToScene) {
                    if (go == null)
                    {
                        result = GetErrorResult(TestName, "The object required doesn't currently exist in the scene", validationTest.Name);
                        List<ResolutionCallback> callbacks = new List<ResolutionCallback>();
                        callbacks.Add(callback);
                        result.Callbacks = callbacks;
                        return result;
                    }
                }

                GameObject actualGO;
                if (ActualValue is Component)
                {
                    actualGO = (ActualValue as Component).gameObject;
                }
                else
                {
                    actualGO = ActualValue as GameObject;
                }

                if (actualGO as UnityEngine.Object == null || !ReferenceEquals(actualGO, go))
                {
                    result = GetWarningResult(TestName, "The value set is not an object in the scene that has been instantiated from the suggested value prefab. This may be OK, in which case click the ignore button.", validationTest.Name);
                    result.RemoveCallbacks();
                    AddDefaultFixCallback(result);
                    return result;
                }
            } else if (!object.Equals(ActualValue, SuggestedValue))
            {
                result = GetWarningResult(TestName, "The value set is not the same as the suggested value. This may be OK, in which case click the ignore checkbox.", validationTest.Name);
                return result;
            }

            return GetPassResult(TestName, validationTest.Name);
        }
    }
}
