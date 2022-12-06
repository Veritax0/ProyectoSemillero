using UnityEngine;

namespace Sonar
{
    public class ScannerControllerPlayer : MonoBehaviour
    {
        public float speed;
        public float delayEndTime;
        private static Transform _player;
        
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
            transform.position = _player.position;
            Vector3 vectorMesh = transform.localScale;
            float growing = speed * Time.deltaTime;
            transform.localScale = new Vector3(vectorMesh.x + growing,vectorMesh.y + growing,vectorMesh.z + growing);
            _audioSource.volume -= Time.fixedDeltaTime * InitialVolume / delayEndTime;
        }

        public static void SetPlayerTransform(Transform playerTransform)
        {
            _player = playerTransform;
        }
        
        private void EndScanner(){
            Destroy(gameObject, delayEndTime);
        }
    }
}
