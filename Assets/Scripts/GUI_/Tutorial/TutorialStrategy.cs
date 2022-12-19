using UnityEngine;

namespace GUI_.Tutorial
{
    public abstract class TutorialStrategy : MonoBehaviour, ITutorialStrategy
    {
        public abstract void Execute();
    }
}