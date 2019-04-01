using UnityEngine;
using wizardscode.digitalpainting.agent;

[CreateAssetMenu(
    fileName = "BaseAgentControllerVariable.asset",
    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Base Agent Controller",
    order = 120)]
public class BaseAgentControllerVariable : BaseVariable<BaseAgentController>
{
}