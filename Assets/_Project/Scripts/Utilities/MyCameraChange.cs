using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class MyCameraChange : MonoBehaviour
    {
        public static Action OnChangeCamera;
        [SerializeField] private GameObject cam1;
        [SerializeField] private GameObject cam2;
        private void OnEnable()
        {
            OnChangeCamera += ChangeCamera;
            cam2.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            OnChangeCamera -= ChangeCamera;
        }

        private void ChangeCamera()
        {
            cam2.gameObject.SetActive(false);
        }
    }
}
