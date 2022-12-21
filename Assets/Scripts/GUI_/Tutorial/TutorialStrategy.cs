using System.Collections;
using UnityEngine;

namespace GUI_.Tutorial
{
    public abstract class TutorialStrategy : MonoBehaviour, ITutorialStrategy
    {
        public abstract void Execute();
        
        protected IEnumerator Freeze()
        {
            yield return new WaitForSeconds(0.6f);
            Time.timeScale = 0;
        }
    }
}