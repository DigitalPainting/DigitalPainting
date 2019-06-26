using System;
using UnityEditor;
using UnityEngine;
using wizardscode.editor;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_MonoBehaviour_SettingSO", menuName = "Wizards Code/Validation/Generic/MonoBehaviour")]
    public class MonoBehaviourSettingSO : AbstractSettingSO<MonoScript>
    {
        [Tooltip("The a PrefabSettingSO (or extension of) that defines the object in the scene that the component should be added to.")]
        [Expandable(isRequired: true)]
        public PrefabSettingSO Target;
               
        protected override MonoScript ActualValue
        {
            get
            {
                if (Target.Instance == null)
                {
                    return null;
                }

                MonoBehaviour obj = Instance;

                if (obj == null)
                {
                    return null;
                }

                return MonoScript.FromMonoBehaviour(obj);
            }
            set
            {
                Target.Instance.AddComponent(Activator.CreateInstance(SuggestedValue.GetClass()).GetType());
            }
        }

        /// <summary>
        /// Get the current Instance of this MonoBehaviour on the target object. Null
        /// if no instance exists.
        /// </summary>
        public MonoBehaviour Instance
        {
            get
            {
                MonoBehaviour obj = null;
                MonoBehaviour[] behaviours = Target.Instance.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (MonoScript.FromMonoBehaviour(behaviour) == SuggestedValue)
                    {
                        obj = behaviour;
                        break;
                    }
                }
                return obj;
            }
        }

        public override void Fix()
        {
            ActualValue = SuggestedValue;
        }

        internal override ValidationResult ValidateSetting(Type validationTest)
        {
            if (Target.Instance == null)
            {
                return GetErrorResult("Target is required.", Target.name, validationTest.Name, new ResolutionCallback(Target.InstantiatePrefab));
            }

            if (ActualValue != null)
            {
                return GetPassResult("MonoBehaviour is required", validationTest.Name);
            }
            else
            {
                return GetErrorResult("MonoBehaviour is required", SuggestedValue.name + " is not present " + Target.Instance.name, validationTest.Name);
            }
        }
    }
}
