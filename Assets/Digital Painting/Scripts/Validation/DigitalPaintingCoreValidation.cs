using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.validation;

namespace wizardscode.digitalpainting
{
    public class DigitalPaintingCoreValidation : ValidationTest<DigitalPaintingManager>
    {
        public override ValidationTest<DigitalPaintingManager> Instance => new DigitalPaintingCoreValidation();

        internal override string ProfileType { get { return "DigitalPaintingManagerProfile"; } }
    }
}
