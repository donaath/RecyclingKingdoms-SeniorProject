using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMap : MonoBehaviour
{
    // Start is called before the first frame update

    [Serializable]
    struct Level
    {
        public string Title;
        public string Details;
        public bool isLocked;

    }
    [SerializeField] Level[] levelsData;
    [SerializeField] Transform points;

    Transform[] levels;
    Transform cam;
    int currentIndex = 0;
    [SerializeField] float speed = .5f;
    [SerializeField] GameObject levelprefab;
    GameObject go;

    void Start()
    {
        cam = Camera.main.transform.parent;
        levels = new Transform[points.childCount];
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = points.GetChild(i);
            go = Instantiate(levelprefab, null);
            go.transform.position = levels[i].position;
            //change level title 
            go.transform.GetChild(0).GetComponent<TMP_Text>().text = levelsData[i].Title;
            //change level Details 
            go.transform.GetChild(1).GetComponent<TMP_Text>().text = levelsData[i].Details;
            //change level visivablirt (access to the game) 
            go.transform.GetChild(3).gameObject.SetActive(!levelsData[i].isLocked); //true if level is unlocked 
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cam.position = Vector2.Lerp(cam.position, levels[currentIndex].position, speed);

    }
    public void ClickRight()
    {
        Move(1);//1:right -1:Left
    }
    public void ClickLeft()
    {
        Move(-1);//1:right -1:Left
    }
    void Move(int dir)
    {
        currentIndex += dir;
        currentIndex = (currentIndex < 0) ? levels.Length - 1 : currentIndex;
        currentIndex = (currentIndex >= levels.Length) ? 0 : currentIndex;
    }

    // to Start AR Game
    public void StartARGame()
    {
        SceneManager.LoadScene("LevelAR"); //when player clcik start it will load to the game session 
    }

}
