using UnityEditor.UIElements;
using UnityEngine;

namespace Sonar
{
    public class ScannerController : MonoBehaviour
    {
        public float speed;
        public float delayEndTime;
        public TagField objetivo;
        
        public AudioClip sonarClip;
        private AudioSource _audioSource;
        private const float InitialVolume = 0.6f;
        
        void Start()
        {
            endScanner();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = sonarClip;
            _audioSource.loop = true;
            _audioSource.volume = InitialVolume;
            _audioSource.Play();
        }

        void Update()
        {
            Vector3 vectorMesh = this.transform.localScale;
            float growing = this.speed * Time.deltaTime;
            this.transform.localScale = new Vector3(vectorMesh.x + growing,vectorMesh.y + growing,vectorMesh.z + growing);
            _audioSource.volume -= Time.fixedDeltaTime * InitialVolume / delayEndTime;
        }

        private void endScanner(){
            Destroy(this.gameObject, delayEndTime);
        }
    }
}
