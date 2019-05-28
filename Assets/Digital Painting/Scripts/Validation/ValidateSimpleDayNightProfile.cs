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

        /**
         * FIXME: move all loging to new SO model
        private void EnableDayNightPlugin()
        {
            GameObject dpManager = GameObject.FindObjectOfType<DigitalPaintingManager>().gameObject;
            dpManager.AddComponent(typeof(DayNightPluginManager));
            SelectDayNightPluginManager();
        }

        void SelectDayNightPluginManager()
        {
            Selection.activeGameObject = Manager.gameObject;
        }
    */
        public override ValidationTest<DayNightPluginManager> Instance => new ValidateSimpleDayNightProfile();

        internal override string ProfileType { get { return "SimpleDayNightProfile"; } }
    }
}