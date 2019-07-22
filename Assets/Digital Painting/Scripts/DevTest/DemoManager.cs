using UnityEngine;
using UnityEngine.EventSystems;

namespace WizardsCode.DevTest
{
    public class DemoManager : MonoBehaviour
    {
        [Tooltip("Prefab for Demo UI")]
        public GameObject demoUI;
        
        void Start()
        {
            // Place the demo UI in the scene	
            GameObject.Instantiate(demoUI);
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject go = new GameObject("EventSystem");
                go.AddComponent<EventSystem>();
                go.AddComponent<StandaloneInputModule>();
            }
        }
    }
}
