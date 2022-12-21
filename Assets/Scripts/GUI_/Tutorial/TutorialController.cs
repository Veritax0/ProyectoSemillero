using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUI_.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        private static TutorialController _instance;

        public List<TutorialStrategy> strategies;
        private int _strategyPos = 0;
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
            GameObject instance = Instantiate(strategies[_strategyPos].gameObject, transform, false);
            _strategy = instance.GetComponent<TutorialStrategy>();
        }
            
        void Update()
        {
            _strategy.Execute();
        }

        public void WaitForStrategy()
        {
            _strategy = _voidStrategy;
            _strategyPos++;
        }

        public void NextStrategy()
        {
            StartCoroutine(Next());
        }

        private IEnumerator Next()
        {
            WaitForStrategy();
            yield return new WaitForSeconds(0.3f);
            GameObject instance = Instantiate(strategies[_strategyPos].gameObject, transform, false);
            _strategy = instance.GetComponent<TutorialStrategy>();
        }
        
    }

    public class VoidStrategy : TutorialStrategy
    {
        public override void Execute() {}
    }
}
