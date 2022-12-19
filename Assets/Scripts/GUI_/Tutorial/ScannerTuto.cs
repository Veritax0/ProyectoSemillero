using System.Collections;
using UnityEngine;

namespace GUI_.Tutorial
{
    public class ScannerTuto : TutorialStrategy
    {
        public Border border;
        private bool _done;
        private Animator _animator;
        private static readonly int Out = Animator.StringToHash("fadeOut");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(Freeze());
        }
        
        public override void Execute()
        {
            if (_done) StartCoroutine(FadeOut());
            
            if (Input.GetKeyDown(KeyCode.Space) && !_done)
            {
                _done = true;
                border.Disappear();
            }
        }
        
        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(1);
            _animator.SetTrigger(Out);
            yield return new WaitForSeconds(2);
            TutorialController.GetInstance().WaitForStrategy();
            Destroy(gameObject);
        }
        
        private IEnumerator Freeze()
        {
            yield return new WaitForSeconds(0.6f);
            Time.timeScale = 0;
        }
    }
}
