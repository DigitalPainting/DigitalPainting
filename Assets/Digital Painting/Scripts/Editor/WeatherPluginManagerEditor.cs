﻿using UnityEngine;
using UnityEditor;
using wizardscode.environment;
using System.Collections.Generic;
using wizardscode.plugin;
using System;
using wizardscode.utility;

namespace wizardscode.editor {
    [CustomEditor(typeof(WeatherPluginManager))]
    public class WeatherPluginManagerEditor : AbstractPluginManagerEditor
    {
    }
}