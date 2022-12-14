using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoreSceneController : MonoBehaviour
{
    public AudioClip narration;
    public Animator panel;
    private AudioSource _audioSource;
    private Coroutine _playCor;
    private bool _isSkip;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _audioSource = GetComponent<AudioSource>();
         _playCor = StartCoroutine(PlayNarration());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) 
            || Input.GetKey(KeyCode.Mouse0) 
            || Input.GetKey(KeyCode.Return))
            StartCoroutine(Skip());
        if (_isSkip)
        {
            _audioSource.volume -= Time.fixedDeltaTime / 6f;
        }
    }

    private IEnumerator PlayNarration()
    {
        yield return new WaitForSeconds(2.5f);
        _audioSource.clip = narration;
        _audioSource.Play();
        yield return new WaitForSeconds(narration.length);
        panel.SetBool("fadeOut", true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Escenario");
    }

    private IEnumerator Skip()
    {
        StopCoroutine(_playCor);
        _isSkip = true;
        panel.SetBool("fadeOut", true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Escenario");
    }
}
