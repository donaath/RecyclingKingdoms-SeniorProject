using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CharacterSelector.Scripts
{
    public class PetManager : SingletonBase<PetManager>
    {
        public PetInfo[] Pets;

        public GameObject SpawnPoint;

        private int _currentIndex = 0;
        private PetInfo _currentPetType = null;
        private PetInfo _currentPet = null;

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
                SetCurrentPetType(_currentIndex);
            }
        }

        // Set the pet type based on index
        public void SetCurrentPetType(int index)
        {
            if (_currentPetType != null)
            {
                Destroy(_currentPetType.gameObject);
            }

            PetInfo pet = Pets[index];
            _currentPetType = Instantiate<PetInfo>(
                pet,
                SpawnPoint.transform.position,
                Quaternion.identity
            );
            _currentIndex = index;
        }

        // Set pet type by name (ID)
        public void SetCurrentPetType(string name)
        {
            int idx = 0;
            foreach (PetInfo petInfo in Pets)
            {
                if (petInfo.PetID.Equals(name, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    SetCurrentPetType(idx); // Correct the method call here
                    break;
                }
                idx++;
            }
        }

        // Create current pet and save it to the database
        public void CreateCurrentPet()
        {
            if (_currentPetType == null)
            {
                Debug.LogError("No pet selected.");
                return;
            }

            _currentPet = Instantiate<PetInfo>(
                _currentPetType,
                SpawnPoint.transform.position,
                Quaternion.identity
            );
            _currentPet.gameObject.SetActive(false); // Optionally keep it inactive

            DontDestroyOnLoad(_currentPet);

            SavePetToDatabase(_currentPet);
        }

        // Get the current pet
        public PetInfo GetCurrentPet()
        {
            return _currentPet;
        }

        // Method to create and save the pet
        public void CreatePet(string petID, string description)
        {
            // Create the pet instance with the given values
            PetInfo newPet = new PetInfo(petID, description);

            // Save the pet to Firebase
            SavePetToDatabase(newPet);
        }

        // Method to save the pet to Firebase
        private void SavePetToDatabase(PetInfo pet)
        {
            string userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("User is not logged in. Cannot save pet.");
                return;
            }

            // Save the pet object in Firebase under the user's ID
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference
                .Child("users")
                .Child(userId)
                .Child("pet")
                .Child(pet.PetID) // Save by petID to prevent overwriting
                .SetRawJsonValueAsync(JsonUtility.ToJson(pet)) // Convert the object to JSON
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("Failed to save pet: " + task.Exception);
                    }
                    else
                    {
                        Debug.Log("Pet saved successfully.");
                    }
                });
        }
    }
}
