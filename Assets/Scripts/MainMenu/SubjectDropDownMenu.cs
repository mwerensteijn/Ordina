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

        private List<string> subjects;
        private List<GameObject> buttonList;
        
        private bool isOpen = false;
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
            for (int i = 0; i < subjects.Count; i++)
            {
                GameObject button = Instantiate(buttonPrefab) as GameObject;
                button.transform.SetParent(gameObject.transform);
                button.GetComponentInChildren<Text>().text = subjects[i];
                button.GetComponent<Button>().onClick.AddListener(
                    () => { gameManager.setSubject(button.GetComponentInChildren<Text>().text);
                            setSubjectButton(button.GetComponentInChildren<Text>().text);
                             closeMenu();
                    }
                );
                buttonList.Add(button);
            }
            isOpen = true;
        }

        private void closeMenu(){
            Debug.Log(buttonList.Count);
            foreach (GameObject button in buttonList)
            {
                Debug.Log(button.GetComponentInChildren<Text>().text);
                Destroy(button);
            }
            buttonList = new List<GameObject>();
            isOpen = false;
        }

        private void setSubjectButton(string text){
            subjectButton.GetComponentInChildren<Text>().text = text;
        }
    }
