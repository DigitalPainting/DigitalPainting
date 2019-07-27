using UnityEngine;

namespace WizardsCode.Animation
{
    /// <summary>
    /// Bounce an object between points.
    /// </summary>
    public class Bounce : MonoBehaviour
    {
        [Tooltip("Maximum height above ground level this thing should bounce to.")]
        public float maxY = 10;
        [Tooltip("Minimum height above ground level this thing should bounce to.")]
        public float minY = 1;
        [Tooltip("The speed at which the thing rises and falls.")]
        public float speed = 1;

        private bool isRising = true;

        void Update()
        {
            Vector3 position = gameObject.transform.position;
            float terrainHeight = UnityEngine.Terrain.activeTerrain.SampleHeight(position);
            if (position.y < terrainHeight + minY)
            {
                isRising = true;
            }
            if (position.y > terrainHeight + maxY)
            {
                isRising = false;
            }

            if (isRising)
            {
                position.y += speed * Time.deltaTime;
            }
            else
            {
                position.y -= speed * Time.deltaTime;
            }

            transform.position = position;
        }
    }
}
