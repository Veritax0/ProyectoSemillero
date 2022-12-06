using UnityEngine;

namespace Sonar
{
    public class ScannerControllerEnemy : MonoBehaviour
    {
        public float speed;
        public float delayEndTime;
        
        public AudioClip sonarClip;
        private AudioSource _audioSource;
        private const float InitialVolume = 0.6f;
        
        void Start()
        {
            EndScanner();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = sonarClip;
            _audioSource.loop = true;
            _audioSource.volume = InitialVolume;
            _audioSource.Play();
        }

        void Update()
        {
            Vector3 vectorMesh = transform.localScale;
            float growing = speed * Time.deltaTime;
            transform.localScale = new Vector3(vectorMesh.x + growing,vectorMesh.y + growing,vectorMesh.z + growing);
            _audioSource.volume -= Time.fixedDeltaTime * InitialVolume / delayEndTime;
        }
        
        private void EndScanner(){
            Destroy(gameObject, delayEndTime);
        }
    }
}
