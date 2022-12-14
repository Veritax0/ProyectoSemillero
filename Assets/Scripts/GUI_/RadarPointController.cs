using UnityEngine;
using UnityEngine.UI;

namespace GUI_
{
    public class RadarPointController : MonoBehaviour
    {
        internal Transform Orientation;
        internal Transform EnemyTransform;
        internal float LifeTime;
        private Image _image;

        private void Start()
        {
            Destroy(gameObject, LifeTime);
            _image = transform.GetChild(0).GetComponent<Image>();
        }

        void Update()
        {
            Vector3 forward = Vector3.Normalize(Orientation.forward);
            Vector3 dirEnemy = Vector3.Normalize(EnemyTransform.position - Orientation.position);
            
            float angle = Vector3.SignedAngle(dirEnemy, forward, Vector3.up);
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Color color = _image.color;
            color.a -= Time.fixedDeltaTime / LifeTime;
            _image.color = color;
        }
    }
}
