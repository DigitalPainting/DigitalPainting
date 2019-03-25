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
        [SerializeField] [Tooltip("The SO Variable Reference indicating which agent currently has focus.")]
        private BaseAgentControllerReference _agentWithFocus;
        [SerializeField] [Tooltip("Main Cinemachine camera rig. If this is left as a null then a Default follow camera setup will be created.")]
        private CinemachineVirtualCameraBase defaultCameraRig;

        private CameraReference _mainCameraReference;

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

        private void Awake()
        {
            SetupMainCamera();
        }

        public void OnAgentWithFocusChanged()
        {
            defaultCameraRig.Follow = _agentWithFocus.Value.transform;
            defaultCameraRig.LookAt = _agentWithFocus.Value.transform;
        }

        /// <summary>
        /// Create the default camera rig. If there is a Main Camera in the scene it will be configured appropriately,
        /// otherwise a camera will be added to the scene.
        /// </summary>
        private void SetupMainCamera()
        {
            if (defaultCameraRig == null)
            {
                GameObject go = new GameObject("Default follow ClearShot");
                defaultCameraRig = go.AddComponent<CinemachineClearShot>();
                defaultCameraRig.m_Priority = 100;

                go = new GameObject("Default follow Virtual Camera");
                go.transform.SetParent(defaultCameraRig.transform);
                CinemachineVirtualCamera vcam = go.AddComponent<CinemachineVirtualCamera>();
                vcam.AddCinemachineComponent<CinemachineTransposer>();
                vcam.AddCinemachineComponent<CinemachineComposer>();
            }

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