using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // UI Variables
    [Header("UI")]
    public GameObject Panel3;
    public GameObject Panel4;
    public GameObject Panel5;
    public GameObject Panel6;
    public GameObject Panel7;
    public GameObject Panel8;

    // Start is called before the first frame update
    private void Awake()
    {
        CreateInstance(); // Create instance of UI manager
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // UI navigation
    public void OpenPanel(GameObject panelToOpen)
    {
        Panel3.SetActive(false);
        Panel4.SetActive(false);
        Panel5.SetActive(false);
        Panel6.SetActive(false);
        Panel7.SetActive(false);
        Panel8.SetActive(false);

        panelToOpen.SetActive(true);
        Debug.Log("Buton Clicked");
    }

    // public void OnLRSceneButtonClick() => SceneManager.LoadScene(0);

    // public void OnCharacterSceneButtonClick() => SceneManager.LoadScene(1);

    // public void OnPetSceneButtonClick() => SceneManager.LoadScene(2);

    public void OnMainSceneButtonClick() => SceneManager.LoadScene(3);

    public void OnStateSceneButtonClick() => SceneManager.LoadScene(4);

    public void OnQuestSceneButtonClick() => SceneManager.LoadScene(5);

    public void OnQMapSceneButtonClick() => SceneManager.LoadScene(6);

    public void OnQSSceneButtonClick() => SceneManager.LoadScene(7);

    public void OnLvlSceneButtonClick() => SceneManager.LoadScene(8);

    public void OnTARSceneButtonClick() => SceneManager.LoadScene(9);

    public void OnLvlARSceneButtonClick() => SceneManager.LoadScene(10);

    public void OnQARSceneButtonClick() => SceneManager.LoadScene(11);

    public void OnCharacterOneButtonClick() => OpenPanel(Panel3);

    public void OnCharacterTwoButtonClick() => OpenPanel(Panel4);

    public void OnCharacterThreeButtonClick() => OpenPanel(Panel5);

    public void OnLoginButtonClick() => OpenPanel(Panel3);

    public void OnRegisterButtonClick() => OpenPanel(Panel4);

    public void OnResetPassButtonClick() => OpenPanel(Panel5);

    public void OnStartButtonClick() => OpenPanel(Panel7);

    public void OnShowNotifButtonClick()
    {
        Panel6.SetActive(true);
    }

    public void OnCloseNotifButtonClick()
    {
        Panel6.SetActive(false);
    }
}
