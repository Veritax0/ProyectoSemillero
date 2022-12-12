using System;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GUI_
{
    public class GuiController : MonoBehaviour
    {
        private static GuiController _instance;
        
        private bool _isPause;
        private bool _restart;
        internal static PlayerMovement PlayerMove;
        
        public RectTransform pauseMenu;
        public Text victoryText;
        public Text defeatText;
        public Text stoleText;
        public EventSystem eventSys;
        
        private RectTransform _pausePanel;
        private RectTransform _pauseText;
        private RectTransform _resumeBtn;
        private RectTransform _restartBtn;
        private RectTransform _mainMenuBtn;
        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(GetComponent<HudController>());
        }

        public static GuiController GetInstance()
        {
            return _instance;
        }

        private void Start()
        {
            _pausePanel = (RectTransform) pauseMenu.GetChild(0);
            _pauseText = (RectTransform)_pausePanel.GetChild(0);
            _resumeBtn = (RectTransform)_pausePanel.GetChild(1);
            _restartBtn = (RectTransform)_pausePanel.GetChild(2);
            _mainMenuBtn = (RectTransform)_pausePanel.GetChild(3);
        }

        private void Update()
        {
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
                eventSys.firstSelectedGameObject = _resumeBtn.gameObject;
            }
            _isPause = !_isPause;
            pauseMenu.gameObject.SetActive(_isPause);
            PlayerMove.enabled = !_isPause;
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
        
        public void Victory()
        {
            victoryText.enabled = true;
            EndGameMenu();
            //_restart = true;
        }

        public void Defeat()
        {
            defeatText.enabled = true;
            EndGameMenu();
            //_restart = true;
        }
        
        public void Stole()
        {
            stoleText.enabled = true;
        }
        public void NotStole()
        {
            stoleText.enabled = false;
        }

        private void EndGameMenu()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            pauseMenu.gameObject.SetActive(true);
            _pauseText.gameObject.SetActive(false);
            _resumeBtn.gameObject.SetActive(false);
            eventSys.firstSelectedGameObject = _restartBtn.gameObject;
        }
    }
}