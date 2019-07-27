using UnityEngine;

namespace WizardsCode.Editor
{
    /// <summary>
    /// Use this property on a ScriptableObject type to allow the editors drawing the field to draw an expandable
    /// area that allows for changing the values on the object without having to change editor.
    /// </summary>
    public class ExpandableAttribute : PropertyAttribute
    {
        public bool Required;
        public string IsRequiredMessage;

        /// <summary>
        /// Mark an attribute as expandable, such attributes will show an inline editor if they have a value.
        /// </summary>
        /// <param name="isRequired">Is the attribute required? If true then an error will be displayed if no value is present</param>
        /// <param name="message">A message to replace the default message if no value is set.</param>
        public ExpandableAttribute(bool isRequired = false, string isRequiredMessage = null)
        {
            this.Required = isRequired;
            this.IsRequiredMessage = isRequiredMessage;
        }
    }
}