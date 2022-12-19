using System.Collections;
using UnityEngine;

namespace GUI_.Tutorial
{
    public class Wasd : TutorialStrategy
    {
        public WasdBorder wBorder;
        public WasdBorder aBorder;
        public WasdBorder sBorder;
        public WasdBorder dBorder;
        
        private Animator _animator;
        private static readonly int Out = Animator.StringToHash("fadeOut");
        
        private bool _wDone;
        private bool _aDone;
        private bool _sDone;
        private bool _dDone;
        private bool _allDone;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            Time.timeScale = 0;
        }

        public override void Execute()
        {
            if (_wDone && _aDone && _sDone && _dDone) _allDone = true;
            
            if (Input.GetKeyDown(KeyCode.W) && !_wDone)
            {
                _wDone = true;
                wBorder.Disappear();
            }
            if (Input.GetKeyDown(KeyCode.A) && !_aDone)
            {
                _aDone = true;
                aBorder.Disappear();
            }
            if (Input.GetKeyDown(KeyCode.S) && !_sDone)
            {
                _sDone = true;
                sBorder.Disappear();
            }
            if (Input.GetKeyDown(KeyCode.D) && !_dDone)
            {
                _dDone = true;
                dBorder.Disappear();
            }

            if (_allDone)
            {
                StartCoroutine(FadeOut());
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
    }
}
