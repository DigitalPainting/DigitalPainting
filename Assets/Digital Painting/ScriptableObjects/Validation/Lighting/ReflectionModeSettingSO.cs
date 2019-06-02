﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "ReflectionModeSettingSO", menuName = "Wizards Code/Validation/Lighting/Reflection Mode")]
    public class ReflectionModeSettingSO : GenericSettingSO<DefaultReflectionMode>
    {
    }
}