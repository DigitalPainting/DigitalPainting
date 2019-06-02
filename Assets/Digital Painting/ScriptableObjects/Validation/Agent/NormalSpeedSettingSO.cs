using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.validation;


namespace wizardscode.agent {
    [CreateAssetMenu(fileName = "NormalSpeedSettingSO", menuName = "Wizards Code/Validation/Agent/Normal Speed")]
    public class NormalSpeedSettingSO : GenericSettingSO<float>
    {
        public override string SettingName 
            { get { return "Normal Movement Speed"; } }

        protected override float ActualValue {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}
