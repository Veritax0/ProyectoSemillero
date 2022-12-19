using UnityEngine;

namespace GUI_.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        private static TutorialController _instance;

        public TutorialStrategy wasd;
        private TutorialStrategy _strategy;
        private VoidStrategy _voidStrategy;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(GetComponent<TutorialController>());
        }

        public static TutorialController GetInstance()
        {
            return _instance;
        }
        
        void Start()
        {
            _voidStrategy = gameObject.AddComponent<VoidStrategy>();
            GameObject instance = Instantiate(wasd.gameObject, transform, false);
            _strategy = instance.GetComponent<TutorialStrategy>();
        }
            
        void Update()
        {
            _strategy.Execute();
        }

        public void WaitForStrategy()
        {
            _strategy = _voidStrategy;
        }
    }

    public class VoidStrategy : TutorialStrategy
    {
        public override void Execute()
        {
        }
    }
}
