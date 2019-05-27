using System.Collections.Generic;
using UnityEngine;
using wizardscode.utility;
using wizardscode.validation;

namespace wizardscode.plugin
{
    public abstract class AbstractPluginProfile : ScriptableObject
    {
        /// <summary>
        /// Test to see if the plugin profile is compatible with the current scene. 
        /// </summary>
        /// <returns>A list of ValidationResults that describe any problems found. If the list is empty then no errors were found.</returns>
        public virtual ValidationResultCollection Validate()
        {
            ValidationResultCollection validations = new ValidationResultCollection();

            IEnumerable<ValidationTest<AbstractPluginManager>> tests = ReflectiveEnumerator.GetEnumerableOfInterfaceImplementors<ValidationTest<AbstractPluginManager>>() as IEnumerable<ValidationTest<AbstractPluginManager>>;
            foreach (ValidationTest<AbstractPluginManager> test in tests)
            {
                validations.AddOrUpdateAll(test.Instance.Execute());
            }

            return validations;
        }
    }
}
