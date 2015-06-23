using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

    class SubjectDropDownMenu: MonoBehaviour
    {
        public dbController db;
        public GameManager gameManager;
        public GameObject buttonPrefab;
        public GameObject subjectButton;

        public MainMenuController controller;

        private List<string> subjects;
        private List<GameObject> buttonList;
        
        private bool isOpen = false;
        private bool subjectPicked = false;
        void Start()
        {
            subjects = db.getSubjects();
            buttonList = new List<GameObject>();
        }

        public void toggleMenu()
        {
            if (isOpen)
            {
                closeMenu();
            }
            else{
                openMenu();
            }
        }

        private void openMenu(){
            int offset = 0;
            for (int i = 0; i < subjects.Count; i++)
            {
                GameObject button = Instantiate(buttonPrefab) as GameObject;
                button.transform.SetParent(gameObject.transform);
                button.transform.position = new Vector3( button.transform.position.x,  button.transform.position.y + offset, button.transform.position.z);
                button.GetComponentInChildren<Text>().text = subjects[i];
                button.GetComponent<Button>().onClick.AddListener(
                    () => { gameManager.setSubject(button.GetComponentInChildren<Text>().text);
                            setSubjectButton(button.GetComponentInChildren<Text>().text);
                             closeMenu();
                    }
                );
                buttonList.Add(button);
                offset -= 30;
            }
            isOpen = true;
        }

        private void closeMenu(){
            foreach (GameObject button in buttonList)
            {
                Destroy(button);
            }
            buttonList = new List<GameObject>();
            isOpen = false;
        }

        private void setSubjectButton(string text){
            subjectPicked = true;
            subjectButton.GetComponentInChildren<Text>().text = text;
        }

        public void next()
        {
            if (subjectPicked)
            {
                controller.selectMiniGame();
            }
            else
            {
               //Popup vul onderwerp in.
            }
        }
    }
