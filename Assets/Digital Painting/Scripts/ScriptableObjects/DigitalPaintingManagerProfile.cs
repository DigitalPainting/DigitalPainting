using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.editor;
using wizardscode.plugin;
using wizardscode.validation;

namespace wizardscode.digitalpainting
{
    /// <summary>
    /// The profile for configuring the core parts of the Digital Painting.
    /// 
    /// Note, while this class extends the AbstractPluginProfile it is not really 
    /// a plugin profile. However, we use this as a base class for convenience as it
    /// allows us to leverage the same `AbstractSettingSO` objects as used to
    /// configure plugins. This includes reusing the validation that comes with
    /// those settings objects.
    /// </summary>
    /// 
    [CreateAssetMenu(fileName = "DigitalPaintingManager_Profile", menuName = "Wizards Code/Digital Painting Profile")]
    public class DigitalPaintingManagerProfile : AbstractPluginProfile
    {
        [Header("Scene Defaults")]
        [Tooltip("The default camera for Digital Painting.")]
        [Expandable(isRequired: true)]
        public PrefabSettingSO DefaultCameraPrefab;

        [Tooltip("The default terrain for a standard Digital Painting.")]
        [Expandable(isRequired: true)]
        public PrefabSettingSO DefaultTerrainPrefab;

        [Header("Agent Defaults")]
        [Tooltip("The default agent for the scene. All scenes should have at least one agent.")]
        [Expandable(isRequired: true)]
        public PrefabSettingSO DefaultAgentPrefab;
    }
}