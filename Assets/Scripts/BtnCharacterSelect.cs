using System.Collections;
using System.Collections.Generic;
using CharacterSelector.Scripts;
using UnityEngine;
using UnityEngine.UI;


namespace CharacterSelector.Scripts
{
    public class BtnCharacterSelect : MonoBehaviour
    {
        public TMPro.TMP_InputField CharacterName;

        // Use this for initialization
        void Start() { }

        // Update is called once per frame
        void Update() { }

    public void SwitchCharacter()
    {
        // e.g. James or Jily
        string characterName = transform.Find("Text").GetComponent<Text>().text;

        CharacterManager.Instance.SetCurrentCharacterType(characterName);

    }

        public void CreateCharacter()
        {
            CharacterManager.Instance.CreateCurrentCharacter(CharacterName.text);
        }
    }
}
