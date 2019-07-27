using System;
using UnityEditor;
using UnityEngine;
using WizardsCode.Editor;
using WizardsCode.Plugin;
using WizardsCode.Validation;

namespace WizardsCode.DigitalPainting.Core.Settings
{
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_Component_SettingSO", menuName = "Wizards Code/Setting/Generic/Component")]
    public class ComponentSettingSO : AbstractSettingSO<Component>
    {
        [Tooltip("The a PrefabSettingSO (or extension of) that defines the object in the scene that the component should be added to.")]
        [Expandable(isRequired: true)]
        public PrefabSettingSO Target;
               
        protected override Component ActualValue
        {
            get
            {
                if (Target.Instance == null)
                {
                    return null;
                }

                Component obj = Instance;

                if (obj == null)
                {
                    return null;
                }

                return (Component)obj;
            }
            set
            {
                Target.Instance.AddComponent(SuggestedValue.GetType());
            }
        }

        /// <summary>
        /// Get the current Instance of this MonoBehaviour on the target object. Null
        /// if no instance exists.
        /// </summary>
        public Component Instance
        {
            get
            {
                Component obj = Target.Instance.GetComponent<Component>();
                return obj;
            }
        }

        public override void Fix()
        {
            ActualValue = SuggestedValue;
        }

        internal override ValidationResult ValidateSetting(Type validationTest, AbstractPluginManager pluginManager)
        {
            if (Target.Instance == null)
            {
                return GetErrorResult("Target is required.", pluginManager, Target.name, validationTest.Name, new ResolutionCallback(Target.InstantiatePrefab));
            }

            if (ActualValue != null)
            {
                return GetPassResult("Component is required", pluginManager, validationTest.Name);
            }
            else
            {
                return GetErrorResult("Component is required", pluginManager, SuggestedValue.name + " is not present " + Target.Instance.name, validationTest.Name);
            }
        }
    }
}
