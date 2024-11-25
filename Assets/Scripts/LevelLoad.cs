using System.Collections;
using System.Collections.Generic;
using CharacterSelector.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelector.Scripts
{
    public class LevelLoad : MonoBehaviour
    {
        // public Text txtType;

        // public Text txtName;

        // public Text txtDescription;

        // public GameObject SpawnPoint;

        // Use this for initialization
        void Start()
        {
            CharacterManager characterManager = CharacterManager.Instance as CharacterManager;

            if (characterManager != null)
            {
                CharacterInfo currentCharacter = characterManager.GetCurrentCharacter();

                if (currentCharacter != null)
                {
                    // currentCharacter.transform.position = SpawnPoint.transform.position;
                    // currentCharacter.gameObject.SetActive(true);

                    // txtType.text = "Type: " + currentCharacter.CharacterID;
                    // txtName.text = "Name: " + currentCharacter.Name;
                    // txtDescription.text = "Description: " + currentCharacter.Description;

                    Debug.Log(currentCharacter.CharacterID);
                    Debug.Log(currentCharacter.Name);
                    Debug.Log(currentCharacter.Description);
                }
                else
                {
                    Debug.LogError("Current character is null.");
                }
            }
            else
            {
                Debug.LogError(
                    "CharacterManager instance is null or could not be cast to CharacterManager."
                );
            }
        }
    }
}
