using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.editor;
using wizardscode.environment;
using wizardscode.utility;

namespace wizardscode.validation
{
    public class ValidateSimpleDayNightProfile : ValidationTest<DayNightPluginManager>
    {
        public override ValidationTest<DayNightPluginManager> Instance => new ValidateSimpleDayNightProfile();

        internal override string ProfileType { get { return "SimpleDayNightProfile"; } }
    }
}