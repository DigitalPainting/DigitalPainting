using UnityEngine;
using UnityEditor;
using WizardsCode.environment;
using System.Collections.Generic;
using WizardsCode.plugin;
using System;
using WizardsCode.utility;

namespace WizardsCode.editor {
    [CustomEditor(typeof(DayNightPluginManager))]
    public class DayNightPluginManagerEditor : AbstractPluginManagerEditor
    {
    }
}