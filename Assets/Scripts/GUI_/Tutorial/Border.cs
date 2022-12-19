using System.Collections;
using UnityEngine;

namespace GUI_.Tutorial
{
    public class Border : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int DoneTrigger = Animator.StringToHash("done");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Disappear()
        {
            StartCoroutine(Done());
        }

        private IEnumerator Done()
        {
            Time.timeScale = 1;
            yield return new WaitForFixedUpdate();
            _animator.SetTrigger(DoneTrigger);
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}