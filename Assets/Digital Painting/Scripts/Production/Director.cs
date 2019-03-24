using Cinemachine;
using UnityEngine;

namespace wizardscode.production
{
    /// <summary>
    /// The Director manages cameras. Each camera will have a priority indicating how important they think
    /// their current shot is. The director will use this to decide which is the correct camera to have
    /// active.
    /// 
    /// When cameras change their priority they should raise a CameraPriorityChangedEvent.
    /// /// </summary>
    public class Director : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The agent that currently has focus.")]
        private BaseAgentControllerReference _agentWithFocus;
        [SerializeField]
        [Tooltip("Main Cinemachine camera rig.")]
        private CinemachineVirtualCameraBase cameraRig;

        private CameraReference _mainCameraReference;
        [SerializeField]
        private CinemachineVirtualCameraBase[] _vCams;

        internal Camera MainCamera
        {
            get
            {
                if (_mainCameraReference.Value == null)
                {
                    SetupMainCamera();
                }
                return _mainCameraReference.Value;
            }
        }

        public void OnCameraPriorityChange()
        {
            int highestPri = -1;
            CinemachineVirtualCameraBase selectedVCam = null;

            for (int i = _vCams.Length - 1; i >= 0; i--)
            {
                if (highestPri < _vCams[i].m_Priority)
                {
                    highestPri = _vCams[i].m_Priority;
                    selectedVCam = _vCams[i];
                }
            }

            selectedVCam.LookAt = _agentWithFocus.Value.transform;
        }

        private void Awake()
        {
            SetupMainCamera();
        }

        public void OnAgentWithFocusChanged()
        {
            cameraRig.Follow = _agentWithFocus.Value.transform;
            cameraRig.LookAt = _agentWithFocus.Value.transform;
        }

        /// <summary>
        /// Create the default camera rig. If there is a Main Camera in the scene it will be configured appropriately,
        /// otherwise a camera will be added to the scene.
        /// </summary>
        private void SetupMainCamera()
        {
            Camera camera = Camera.main;
            CinemachineBrain brain = camera.GetComponent<CinemachineBrain>();
            if (brain == null)
            {
                Debug.LogWarning("Camera did not have a Cinemachine brain, adding one. You should probably add one to your camera in the scene.");
                brain = camera.gameObject.AddComponent<CinemachineBrain>();
            }
            
            if (camera.GetComponent<AudioListener>() == null)
            {
                Debug.LogWarning("Camera did not have an audio listener, adding one. You should probably add one to your camera in the scene.");
                camera.gameObject.AddComponent<AudioListener>();
            }

            if (camera.GetComponent<FlareLayer>() == null)
            {
                Debug.LogWarning("Camera did not have an Flare Layer, adding one. You should probably add one to your camera in the scene.");
                camera.gameObject.AddComponent<FlareLayer>();
            }
        }
    }
}