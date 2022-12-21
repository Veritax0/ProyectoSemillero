using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GUI_.Tutorial
{
    public class ScannerTuto : TutorialStrategy
    {
        public Border border;
        public Text text;
        public Animator arrowAnimator;

        private Transform _arrowTransform;
        private bool _spaceDone;
        private bool _spaceReleased;
        private Func<bool> _spaceReleasedFunc;
        private Animator _animator;
        private static readonly int FadeOut = Animator.StringToHash("fadeOut");
        private static readonly int FadeIn = Animator.StringToHash("fadeIn");

        private void Start()
        {
            _spaceDone = false;
            _animator = GetComponent<Animator>();
            StartCoroutine(Freeze());
            _spaceReleasedFunc = SpaceReleased;
            _arrowTransform = arrowAnimator.gameObject.GetComponent<Transform>();
        }
        
        public override void Execute()
        {
            if (Input.GetKeyUp(KeyCode.Space)) _spaceReleased = true;
            
            if (Input.GetKeyDown(KeyCode.Space) && !_spaceDone)
            {
                _spaceDone = true;
                border.Disappear();
                StartCoroutine(SpaceDone());
            }
        }
        private bool SpaceReleased()
        {
            return _spaceReleased;
        }
        
        private IEnumerator SpaceDone()
        {
            yield return new WaitForSeconds(1);
            
            _animator.SetTrigger(FadeOut);
            yield return new WaitForSeconds(1.5f);
            
            _animator.ResetTrigger(FadeOut);
            yield return new WaitUntil(_spaceReleasedFunc);
            
            String txt = "la barra de capacidad" +
                         " se llenara automaticamente mientras" +
                         " no se use la habilidad de scanner";
            
            StartCoroutine(ChangeText(txt));
            yield return new WaitForSeconds(5);
            
            _arrowTransform.localPosition = new Vector3(-490, -340, 0);
            txt = "la barra de sobrecarga" +
                  " comenzara a bajar mientras" +
                  " se use la habilidad de scanner sin capacidad\n\n" +
                  " al vaciarse la barra de sobrecarga" +
                  " no se podra utilizar la habilidad durante unos segundos";
            StartCoroutine(ChangeText(txt));
            yield return new WaitForSeconds(5);
            
            TutorialController.GetInstance().WaitForStrategy();
            Destroy(gameObject);
        }

        private IEnumerator ChangeText(String txt)
        {
            text.text = txt;
            _animator.SetTrigger(FadeIn);
            yield return new WaitForSeconds(0.5f);
            
            arrowAnimator.SetTrigger(FadeIn);
            _animator.ResetTrigger(FadeIn);
            yield return new WaitForSeconds(3);
            
            arrowAnimator.ResetTrigger(FadeIn);
            _animator.SetTrigger(FadeOut);
            arrowAnimator.SetTrigger(FadeOut);
            yield return new WaitForSeconds(1.5f);
            
            _animator.ResetTrigger(FadeOut);
            arrowAnimator.ResetTrigger(FadeOut);
        }
    }
}
