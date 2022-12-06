using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoreSceneController : MonoBehaviour
{
    public AudioClip narration;
    public Animator panel;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayNarration());
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
}
