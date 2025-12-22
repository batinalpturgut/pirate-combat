using UnityEngine;

namespace Root.Scripts.Utilities
{
    public class MousePos : MonoBehaviour
    {
        [SerializeField]
        private LayerMask mousePlaneLayerMask;

        private static MousePos _instance;


        private void Awake()
        {
            _instance = this;
        }


        private void Update()
        {
            transform.position = GetPosition();
        }

        public static Vector3 GetPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _instance.mousePlaneLayerMask);

            return new Vector3(raycastHit.point.x, 0.1f, raycastHit.point.z);
        }
    }
}