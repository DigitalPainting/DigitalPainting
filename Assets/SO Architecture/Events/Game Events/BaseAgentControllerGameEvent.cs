using ScriptableObjectArchitecture;
using UnityEngine;
using WizardsCode.digitalpainting.agent;

[System.Serializable]
[CreateAssetMenu(
    fileName = "BaseAgentControllerGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Base Agent Controller",
    order = 120)]
public sealed class BaseAgentControllerGameEvent : GameEventBase<BaseAgentController>
{
}