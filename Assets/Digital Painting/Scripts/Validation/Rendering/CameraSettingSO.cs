using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_CameraSettingSO", menuName = "Wizards Code/Validation/Camera")]
    public class CameraSettingSO : PrefabSettingSO
    {
        [Header("Camera Settings")]
        [Tooltip("The Post Processing Profile to add to use on the camera.")]
        public PostProcessProfile postProcessingProfile;

        internal override ValidationResult ValidateSetting(Type validationTest)
        {
            ValidationResult result = base.ValidateSetting(validationTest);
            if (result.impact != ValidationResult.Level.OK)
            {
                return result;
            }

            GameObject go = ((GameObject)ActualValue);
            if (postProcessingProfile)
            {
                PostProcessVolume volume = go.GetComponent<PostProcessVolume>();
                if (volume == null)
                {
                    return GetErrorResult(TestName, "Camera does not have a post processing volume.", validationTest.Name, new ResolutionCallback(AddPostProcessing));
                }

                if (volume.profile != postProcessingProfile) {
                    return GetErrorResult(TestName, "Camera does not have the correct post processing volume.", validationTest.Name, new ResolutionCallback(AddPostProcessingProfile));
                }
            }
            return GetPassResult(TestName, validationTest.Name);
        }

        void AddPostProcessing()
        {
            GameObject go = (GameObject)ActualValue;

            PostProcessVolume volume = go.AddComponent<PostProcessVolume>();
            AddPostProcessingProfile();

            PostProcessLayer postProcessLayer = go.AddComponent<PostProcessLayer>();
            postProcessLayer.volumeLayer = LayerMask.NameToLayer("PostProcessing");
            postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;

            //postProcessLayer.Init(resources);
        }

        private void AddPostProcessingProfile()
        {
            GameObject go = (GameObject)ActualValue;

            PostProcessVolume volume = go.GetComponent<PostProcessVolume>();
            
            volume.isGlobal = true;
            volume.profile = postProcessingProfile;
        }
    }
}
