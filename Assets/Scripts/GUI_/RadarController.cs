using UnityEngine;

namespace GUI_
{
    public class RadarController : MonoBehaviour
    {
        private static RadarController _instance;

        public GameObject canvas;
        public RadarPointController bladeRadarPoint;
        public RadarPointController gunRadarPoint;
        public Transform playerOrientation;
        public float radarPointLifeTime;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(GetComponent<RadarController>());
        }

        public static RadarController GetInstance()
        {
            return _instance;
        }

        public void AddBladePoint(Transform enemyTransform)
        {
            GameObject obj = Instantiate(bladeRadarPoint.gameObject, transform, false);
            RadarPointController point = obj.GetComponent<RadarPointController>();
            point.LifeTime = radarPointLifeTime;
            point.Orientation = playerOrientation;
            point.EnemyTransform = enemyTransform;
        }

        public void AddGunPoint(Transform enemyTransform)
        {
            GameObject obj = Instantiate(gunRadarPoint.gameObject, transform, false);
            RadarPointController point = obj.GetComponent<RadarPointController>();
            point.LifeTime = radarPointLifeTime;
            point.Orientation = playerOrientation;
            point.EnemyTransform = enemyTransform;
        }
    }
}
