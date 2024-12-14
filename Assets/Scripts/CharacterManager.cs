using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CharacterSelector.Scripts
{
    public class CharacterManager : SingletonBase<CharacterManager>
    {
        public CharacterInfo[] Characters;
        public GameObject SpawnPoint;

        private int _currentIndex = 0;
        private CharacterInfo _currentCharacterType = null;

        private CharacterInfo _currentCharacter = null;

        private FirebaseDatabase _database;
        private DatabaseReference _databaseReference;

        protected override void Init()
        {
            Persist = true;
            base.Init();

            // Initialize Firebase Database
            _database = FirebaseDatabase.DefaultInstance;
            _databaseReference = _database.RootReference;
        }

        public void Start()
        {
            if (SpawnPoint != null)
            {
                SetCurrentCharacterType(_currentIndex);
            }
        }

        // Set the character type based on index
        public void SetCurrentCharacterType(int index)
        {
            if (_currentCharacterType != null)
            {
                Destroy(_currentCharacterType.gameObject);
            }

            CharacterInfo character = Characters[index];
            _currentCharacterType = Instantiate<CharacterInfo>(
                character,
                SpawnPoint.transform.position,
                Quaternion.identity
            );
            _currentIndex = index;
        }

        // Set character type by name (ID)
        public void SetCurrentCharacterType(string name)
        {
            int idx = 0;
            foreach (CharacterInfo characterInfo in Characters)
            {
                if (
                    characterInfo.CharacterID.Equals(
                        name,
                        System.StringComparison.InvariantCultureIgnoreCase
                    )
                )
                {
                    SetCurrentCharacterType(idx);
                    break;
                }
                idx++;
            }
        }

        // Create current character and save it to the database
        public void CreateCurrentCharacter()
        {
            if (_currentCharacterType == null)
            {
                Debug.LogError("No character selected.");
                return;
            }

            _currentCharacter = Instantiate<CharacterInfo>(
                _currentCharacterType,
                SpawnPoint.transform.position,
                Quaternion.identity
            );
            _currentCharacter.gameObject.SetActive(false); // Optionally keep it inactive

            DontDestroyOnLoad(_currentCharacter);

            SaveCharacterToDatabase(_currentCharacter);
        }

        // Get the current character
        public CharacterInfo GetCurrentCharacter()
        {
            return _currentCharacter;
        }

        // Method to create and save the character
        public void CreateCharacter(string characterID, string description)
        {
            // Create the character instance with the given values
            CharacterInfo newCharacter = new CharacterInfo(characterID, description);

            // Save the character to Firebase
            SaveCharacterToDatabase(newCharacter);
        }

        // Method to save the character to Firebase
        private void SaveCharacterToDatabase(CharacterInfo character)
        {
            string userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("User is not logged in. Cannot save character.");
                return;
            }

            // Save the character object in Firebase under the user's ID
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference
                .Child("users")
                .Child(userId)
                .Child("character")
                .Child(character.CharacterID) // Save by characterID to prevent overwriting
                .SetRawJsonValueAsync(JsonUtility.ToJson(character)) // Convert the object to JSON
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("Failed to save character: " + task.Exception);
                    }
                    else
                    {
                        Debug.Log("Character saved successfully.");
                    }
                });
        }
    }
}
