using System.Collections;
using System.Collections.Generic;
using CharacterSelector.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CharacterSelector.Scripts
{
    public class BtnPetSelect : MonoBehaviour
    {
        // Use this for initialization
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void SwitchPet()
        {
            // e.g. Bunny or Kitty or Puppy
            string petName = transform.Find("Text").GetComponent<Text>().text;

            PetManager.Instance.SetCurrentPetType(petName);
        }

        public void CreatePet()
        {
            PetManager.Instance.CreateCurrentPet();
            SceneManager.LoadScene(3);
        }
    }
}
