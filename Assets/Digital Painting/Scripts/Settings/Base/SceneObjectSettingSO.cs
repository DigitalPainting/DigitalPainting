using System;
using System.Reflection;
using UnityEngine;

namespace WizardsCode.validation
{
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_SceneObject_SettingSO", menuName = "Wizards Code/Validation/Generic/Scene Object")]
    public class SceneObjectSettingSO : GenericSettingSO<GameObject>
    {
        public PrefabSettingSO prefabSetting;

        [HideInInspector]
        public override GameObject SuggestedValue
        {
            get {
                return prefabSetting.Instance;
            }
        }

        protected override GameObject ActualValue {
            get
            {
                if (Accessor == null)
                {
                    throw new Exception("No accessor set and ActualValue getter is not overriden in " + GetType());
                }

                object value;
                if (Accessor.MemberType == MemberTypes.Property)
                {
                    value = ((PropertyInfo)Accessor).GetValue(default(QualitySettings));
                }
                else
                {
                    value = ((FieldInfo)m_Accessor).GetValue(default(QualitySettings));
                }

                return ConvertToGameObject((UnityEngine.Object)value);
            }
            set
            {
                if (Accessor.MemberType == MemberTypes.Property)
                {
                    
                    ((PropertyInfo)Accessor).SetValue(default(QualitySettings), 
                        value.GetComponent(((PropertyInfo)Accessor).PropertyType));
                }
                else
                {
                    ((FieldInfo)Accessor).SetValue(default(QualitySettings), 
                        value.GetComponent(((FieldInfo)Accessor).FieldType));
                }
            }
        }
    }
}
