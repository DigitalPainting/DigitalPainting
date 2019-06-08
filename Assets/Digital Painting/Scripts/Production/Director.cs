using Cinemachine;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.validation;

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

        private CinemachineVirtualCameraBase cameraRig;
    private GameObject followClearshotGO;

        private BaseAgentController _agentWithFocus;

        private void Awake()
        {
            SetupMainCamera();
            AgentWithFocus = FindObjectOfType<BaseAgentController>();
        }

        /// <summary>
        /// Gets the current agent with the directors focus, this is what cameras will
        /// look at unless the Director is giving some other instruction at the time.
        /// </summary>
        public BaseAgentController AgentWithFocus
        {
            get {
                return _agentWithFocus;
            }
            set
            {
                if (_agentWithFocus != value)
                {
                    _agentWithFocus = value;
                    cameraRig.Follow = _agentWithFocus.transform;
                    cameraRig.LookAt = _agentWithFocus.transform;

                    SetCameraAimMode();
                }
            }
        }

        private void SetCameraAimMode()
        {
            CinemachineVirtualCamera vcam = followClearshotGO.GetComponent<CinemachineVirtualCamera>();
            CinemachineComponentBase current = vcam.GetCinemachineComponent(CinemachineCore.Stage.Aim);
            if (current.GetType() == _agentWithFocus.Settings.cameraAimMode.GetType())
            {
                return;
            } else
            {
                // Destroy the existing component (have to try to destroy all as its a generic method)
                vcam.DestroyCinemachineComponent<CinemachineHardLookAt>();
                vcam.DestroyCinemachineComponent<CinemachineGroupComposer>();
                vcam.DestroyCinemachineComponent<CinemachineHardLookAt>();
                vcam.DestroyCinemachineComponent<CinemachineHardLookAt>();
                vcam.DestroyCinemachineComponent<CinemachinePOV>();
            }

            switch (_agentWithFocus.Settings.cameraAimMode)
            {
                case AgentSettingSO.CameraAimMode.Composer:
                    vcam.AddCinemachineComponent<CinemachineComposer>();
                    break;
                case AgentSettingSO.CameraAimMode.GroupComposer:
                    vcam.AddCinemachineComponent<CinemachineGroupComposer>();
                    break;
                case AgentSettingSO.CameraAimMode.HardLookAt:
                    vcam.AddCinemachineComponent<CinemachineHardLookAt>();
                    break;
                case AgentSettingSO.CameraAimMode.POV:
                    vcam.AddCinemachineComponent<CinemachinePOV>();
                    break;
                case AgentSettingSO.CameraAimMode.SameAsFollowTarget:
                    vcam.AddCinemachineComponent<CinemachineSameAsFollowTarget>();
                    break;
                default:
                    Debug.LogError("Sorry, don't know how to use a camera Aim policy of " + _agentWithFocus.Settings.cameraAimMode);
                    break;
            }
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
                    cameraRig = defaultCameraSetup;
                }
                else
                {
                    cameraRig = Instantiate(defaultCameraSetup);
                    cameraRig.gameObject.name = "Default Cinemachine ClearShot Camera";
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
            followClearshotGO = new GameObject("Default follow ClearShot");

            cameraRig = followClearshotGO.AddComponent<CinemachineClearShot>();
            cameraRig.m_Priority = 100;
            followClearshotGO.AddComponent<CinemachineCollider>();

            followClearshotGO = new GameObject("Default follow Virtual Camera");
            followClearshotGO.transform.SetParent(cameraRig.transform);
            CinemachineVirtualCamera vcam = followClearshotGO.AddComponent<CinemachineVirtualCamera>();

            CinemachineTransposer transposer = vcam.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y = 1.8f;
            transposer.m_FollowOffset.z = -6;
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;

            vcam.AddCinemachineComponent<CinemachineHardLookAt>();
        }
    }
}