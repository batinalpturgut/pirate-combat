using Root.Scripts.Utilities.Curves;
using UnityEngine;

namespace Root.Scripts
{
    public class SplineTest : MonoBehaviour
    {
        [SerializeField]
        private SplineComponent splineComponent;

        void Start()
        {
            splineComponent.GetTween(transform);
        }
    }
}