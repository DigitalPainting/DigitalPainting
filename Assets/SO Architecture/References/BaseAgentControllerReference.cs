using System;
using UnityEngine;
using wizardscode.digitalpainting.agent;

[Serializable]
public sealed class BaseAgentControllerReference : BaseReference<BaseAgentController, BaseAgentControllerVariable>
{
    public BaseAgentControllerReference() : base() { }
    public BaseAgentControllerReference(BaseAgentController value) : base(value) { }

    public override BaseAgentController Value {
        get => base.Value;
        set
        {
            base.Value = value;
            if (_variable.onChangeEvent != null)
            {
                _variable.onChangeEvent.Raise();
            }
        }
    }
}