using UnityEngine;
using System.Collections;

    public class MainMenuController: MonoBehaviour
    {
        private GameObject lastPanel;
        public GameObject mainMenuPanel;
        public GameObject subjectPanel;
        public GameObject miniGamePanel;
        public GameObject playerPanel;

        void Start()
        {
            lastPanel = mainMenuPanel;
            selectMainMenu();
        }

        public void selectMainMenu()
        {
            lastPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            lastPanel = mainMenuPanel;
        }

        public void selectSubject()
        {
            lastPanel.SetActive(false);
            subjectPanel.SetActive(true);
            lastPanel = subjectPanel;
        }

        public void selectMiniGame()
        {
            lastPanel.SetActive(false);
            miniGamePanel.SetActive(true);
            lastPanel = miniGamePanel;
        }


        public void selectPlayerName()
        {
            lastPanel.SetActive(false);
            playerPanel.SetActive(true);
            lastPanel = playerPanel;
        }

        public void quitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
