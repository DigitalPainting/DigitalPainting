using Cinemachine;
using UnityEngine;
using wizardscode.digitalpainting.agent;

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
        [SerializeField] [Tooltip("Main Cinemachine camera rig. This can either be a rig in the scene, a prefab that will be instantiated or null. In the case of a null a Default follow camera setup will be created.")]
        private CinemachineVirtualCameraBase defaultCameraSetup;
        [SerializeField]

        private CinemachineVirtualCameraBase defaultCameraRig;
        private BaseAgentController _agentWithFocus;

        private void Awake()
        {
            SetupMainCamera();
        }

        public BaseAgentController AgentWithFocus
        {
            get { return _agentWithFocus; }
            set
            {
                if (_agentWithFocus != value)
                {
                    _agentWithFocus = value;
                    OnAgentWithFocusChanged();
                }
            }
        }

        public void OnAgentWithFocusChanged()
        {
            defaultCameraRig.Follow = _agentWithFocus.transform;
            defaultCameraRig.LookAt = _agentWithFocus.transform;
        }

        /// <summary>
        /// Create the default camera rig. If there is a Main Camera in the scene it will be configured appropriately,
        /// otherwise a camera will be added to the scene.
        /// </summary>
        private void SetupMainCamera()
        {
            if (defaultCameraSetup == null)
            {
                CreateDefaultClearShot();
            }
            else
            {
                if (defaultCameraSetup.gameObject.scene.IsValid())
                {
                    defaultCameraRig = defaultCameraSetup;
                }
                else
                {
                    defaultCameraRig = Instantiate(defaultCameraSetup);
                    defaultCameraRig.gameObject.name = "Default Cinemachine ClearShot Camera";
                }
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

        private void CreateDefaultClearShot()
        {
            GameObject go = new GameObject("Default follow ClearShot");
            defaultCameraRig = go.AddComponent<CinemachineClearShot>();
            defaultCameraRig.m_Priority = 100;
            go.AddComponent<CinemachineCollider>();

            go = new GameObject("Default follow Virtual Camera");
            go.transform.SetParent(defaultCameraRig.transform);
            CinemachineVirtualCamera vcam = go.AddComponent<CinemachineVirtualCamera>();

            CinemachineFramingTransposer transposer = vcam.AddCinemachineComponent<CinemachineFramingTransposer>();
            transposer.m_DeadZoneWidth = 0.3f;
            transposer.m_DeadZoneHeight = 0.35f;
            transposer.m_DeadZoneDepth = 3f;

            CinemachineComposer composer = vcam.AddCinemachineComponent<CinemachineComposer>();
            composer.m_DeadZoneWidth = 0.5f;
            composer.m_DeadZoneWidth = 0.5f;
        }
    }
}