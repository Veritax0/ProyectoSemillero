using System;
using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GUI_
{
    public class HudController : MonoBehaviour
    {
        public List<Image> hits;
        public Image sonarCapacityBar;
        public Image runCapacityBar;
        public Image minRunCap;
        public Image sonarOverload;
        public Image playerStatus;
        public Sprite idleSprite;
        public Sprite runSprite;
        public Sprite squatSprite;
        public Sprite walkSprite;
        public float hitDecreaseSec = 1;
        
        private int _currentHit;
        private static HudController _instance;
        private bool _isRemovingHit;
        private float _maxOverload;
        internal static PlayerMovement PlayerMovement;

        public Text victoryText;
        public Text defeatText;
        public Text stoleText;
        public RectTransform pauseMenu;
        
        private bool _restart;
        private bool _isPause;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(GetComponent<HudController>());
        }

        public static HudController GetInstance()
        {
            return _instance;
        }
        
        private void Start()
        {
            _currentHit = hits.Count - 1;
        }

        private void Update()
        {
            if(_isRemovingHit) RemoveAHit();
            if (Input.GetKeyDown(KeyCode.Escape) && _restart)
            {
                SceneManager.LoadScene("Escenario");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Pause();
            }
        }

        private void Pause()
        {
            if (_isPause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            _isPause = !_isPause;
            pauseMenu.gameObject.SetActive(_isPause);
            PlayerMovement.enabled = !_isPause;
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Escenario");
        } 
        
        public void Resume()
        {
            Pause();
        } 
        
        public void MainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        } 
        
        public void UpdateSonarBar(float value)
        {
            sonarCapacityBar.fillAmount = value / 100;
        }
        
        public void UpdateRunBar(float value)
        {
            runCapacityBar.fillAmount = value / 100;
        }

        public void RemoveHit()
        {
            _isRemovingHit = true;
        }

        private void RemoveAHit()
        {
            if (_currentHit < 0) return;
            Image hit = hits[_currentHit];
            float frameTime = Time.fixedDeltaTime;
            float decrease = frameTime * hitDecreaseSec;
            float newCapacity = hit.fillAmount - decrease;
            newCapacity = newCapacity >= 0 ? newCapacity : 0;
            hit.fillAmount = newCapacity;
            if (newCapacity == 0)
            {
                _isRemovingHit = false;
                _currentHit--;
            }
        }

        public void UpdateInverseOverload(float value, float max)
        {
            _maxOverload = max;
            float newAmount = 1 - (value / _maxOverload) ;
            sonarOverload.fillAmount = newAmount;
        }

        public void UpdateOverload(float value)
        {
            float newAmount = value / _maxOverload ;
            sonarOverload.fillAmount = newAmount;
        }

        public void SetMinRunCapacity(float value)
        {
            //minRunCap.rectTransform.position += new Vector3(value, 0, 0);
            minRunCap.transform.position += new Vector3(0, value, 0);
        }

        public void UpdatePlayerStatus(PlayerMoveStatus status)
        {
            switch (status)
            {
                case PlayerMoveStatus.Running:
                    playerStatus.sprite = runSprite;
                    break;
                case PlayerMoveStatus.Squatting:
                    playerStatus.sprite = squatSprite;
                    break;
                case PlayerMoveStatus.Idle:
                    playerStatus.sprite = idleSprite;
                    break;
                case PlayerMoveStatus.Walking:
                    playerStatus.sprite = walkSprite;
                    break;
            }
        }

        public void Victory()
        {
            victoryText.enabled = true;
            _restart = true;
        }

        public void Defeat()
        {
            defeatText.enabled = true;
            _restart = true;
        }
        
        public void Stole()
        {
            stoleText.enabled = true;
        }
        public void NotStole()
        {
            stoleText.enabled = false;
        }
    }
}
