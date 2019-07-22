using ScriptableObjectArchitecture;
using UnityEngine;

namespace WizardsCode.DigitalPainting.Agent
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "BaseAgentControllerGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "Base Agent Controller",
        order = 120)]
    public sealed class BaseAgentControllerGameEvent : GameEventBase<BaseAgentController>
    {
    }
}