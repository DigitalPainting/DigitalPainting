using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.digitalpainting.agent;

public class DroneControlUI : MonoBehaviour {
    [Header("UI Elements")]
    [Tooltip("Toggle for turning AI Fly By Wire control on/off")]
    public Toggle flyByWireToggle;

    private DroneController droneController;

    private void Awake()
    {
        droneController = FindObjectOfType<DroneController>();
    }

    private void Start()
    {
        flyByWireToggle.isOn = droneController.isFlyByWire;
    }

    public void OnFlyByWireToggleChanged()
    {
        droneController.isFlyByWire = flyByWireToggle.isOn;
    }
}
