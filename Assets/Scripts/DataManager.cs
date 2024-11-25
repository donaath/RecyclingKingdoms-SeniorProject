using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelector.Scripts
{
    public class DataManager : MonoBehaviour
    {
        public TMP_InputField nameField;
        public TMP_InputField groupField;
        public string userID;
        DatabaseReference dbsRef;

        private void Awake()
        {
            userID = SystemInfo.deviceUniqueIdentifier;
            dbsRef = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void CreateUser()
        {
            User newUser = new User(nameField, groupField);
            string json = JsonUtility.ToJson(newUser);
            dbsRef.Child("users").Child(userID).Child("username").SetRawJsonValueAsync(json);
        }
    }

    [System.Serializable]
    public class User
    {
        public string name;
        public string group;

        // Constructor to initialize user data
        public User(TMP_InputField nameField, TMP_InputField groupField)
        {
            this.name = nameField.text;
            this.group = groupField.text;
        }
    }
}
