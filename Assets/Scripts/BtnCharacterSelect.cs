using System.Collections;
using System.Collections.Generic;
using CharacterSelector.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CharacterSelector.Scripts
{
    public class BtnCharacterSelect : MonoBehaviour
    {
        public void SwitchCharacter()
        {
            // e.g. James or Jily
            string characterName = transform.Find("Text").GetComponent<Text>().text;

            CharacterManager.Instance.SetCurrentCharacterType(characterName);
        }

        public void CreateCharacter()
        {
            CharacterManager.Instance.CreateCurrentCharacter();
            SceneManager.LoadScene(2);
        }
    }
}
