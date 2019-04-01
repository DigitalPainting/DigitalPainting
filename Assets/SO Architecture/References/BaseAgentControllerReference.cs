using System;
using UnityEngine;
using wizardscode.digitalpainting.agent;

[Serializable]
public sealed class BaseAgentControllerReference : BaseReference<BaseAgentController, BaseAgentControllerVariable>
{
    Cinemachine.CinemachineVirtualCameraBase vCam;

    public BaseAgentControllerReference() : base() { }
    public BaseAgentControllerReference(BaseAgentController value) : base(value) { }

    public override BaseAgentController Value {
        get => base.Value;
        set
        {
            if (vCam != null)
            {
                vCam.Priority = 10;
            }

            base.Value = value;
            
            if (Value.virtualCameraPrefab != null && vCam == null)
            {
                vCam = GameObject.Instantiate(Value.virtualCameraPrefab);
            }

            if (vCam != null)
            {
                vCam.Follow = Value.transform;
                vCam.LookAt = Value.transform;
                vCam.Priority = 150;
            }

            if (_variable.onChangeEvent != null)
            {
                _variable.onChangeEvent.Raise();
            }
        }
    }
}