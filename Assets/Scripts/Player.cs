using System.Collections;
using System.Text.RegularExpressions;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CharacterSelector.Scripts
{
    public class Player : MonoBehaviour
    {
        [Header("Firebase")]
        public DependencyStatus dependencyStatus;
        private FirebaseAuth auth; // Firebase Authentication instance
        private FirebaseDatabase dbs;
        private DatabaseReference databaseReference;

        // Login Variables
        [Space]
        [Header("Login")]
        public TMP_InputField emailLoginField;
        public TMP_InputField passwordLoginField;

        // Registration Variables
        [Space]
        [Header("Registration")]
        public TMP_InputField userNameRegisterField;
        public TMP_InputField ageRegisterField;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;

        // Reset Password Variables
        [Space]
        [Header("Reset Password")]
        public TMP_InputField passwordResetField;

        // Error Message Variables
        [Space]
        [Header("Error Message")]
        public TMP_Text notifTitleField;
        public TMP_Text notifMessageField;

        // Password Strength Variable
        [Space]
        [Header("Password Strength Display")]
        public TMP_Text passwordStrengthText;

        private void Awake()
        {
            FirebaseApp
                .CheckAndFixDependenciesAsync()
                .ContinueWithOnMainThread(task =>
                {
                    dependencyStatus = task.Result;
                    if (dependencyStatus == DependencyStatus.Available)
                    {
                        InitializeFirebase();
                    }
                    else
                    {
                        Debug.LogError(
                            $"Could not resolve all Firebase dependencies: {dependencyStatus}"
                        );
                    }
                });

            if (passwordRegisterField != null)
            {
                passwordRegisterField.onValueChanged.AddListener(OnPasswordChanged); // Password strength
            }
        }

        void InitializeFirebase()
        {
            auth = FirebaseAuth.DefaultInstance; // Initialize Firebase Auth
            dbs = FirebaseDatabase.DefaultInstance;
            databaseReference = dbs.RootReference; // Initialize Database Reference
            Debug.Log("Firebase Realtime Database Initialized");
        }

        // Login Function
        public void Login()
        {
            StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
        }

        private IEnumerator LoginAsync(string email, string password)
        {
            var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                Debug.LogError(loginTask.Exception);

                FirebaseException firebaseException =
                    loginTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Login Failed! ";
                string errorTitle = "LOGIN ERROR";

                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is Invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is Missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is Missing";
                        break;
                    default:
                        failedMessage = "Login Failed";
                        break;
                }

                Debug.Log(failedMessage);
                ShowNotification(errorTitle, failedMessage);
            }
            else
            {
                var authResult = loginTask.Result;
                Debug.LogFormat("{0} You Are Successfully Logged In", authResult.User.DisplayName);
            }
        }

        // Sign out function
        public void SignOut()
        {
            if (auth != null)
            {
                auth.SignOut();
                Debug.Log("User has been signed out successfully.");
                ShowNotification("Sign Out", "You have been signed out.");

                // Optional: Redirect to login screen or perform other actions
                // For example, load a login scene
                // SceneManager.LoadScene("LoginSceneName");
            }
            else
            {
                Debug.LogError("Firebase Auth is not initialized.");
                ShowNotification("Sign Out Error", "Sign Out Failed. Please try again.");
            }
        }

        // Registration Function
        public void Register()
        {
            StartCoroutine(
                RegisterAsync(
                    userNameRegisterField.text,
                    emailRegisterField.text,
                    passwordRegisterField.text,
                    ageRegisterField.text
                )
            );
        }

        private IEnumerator RegisterAsync(string name, string email, string password, string age)
        {
            int parsedAge = 0;
            if (
                string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(age)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(password)
            )
            {
                Debug.LogError("All Fields Are Required");
                ShowNotification("REGISTRATION ERROR", "All Fields Are Required");
                yield break;
            }
            else if (!int.TryParse(age, out parsedAge))
            {
                Debug.LogError("Invalid Age");
                ShowNotification("REGISTRATION ERROR", "Age Must Be A Valid Integer");
                yield break;
            }
            else if (!IsValidEmail(email))
            {
                Debug.LogError("Invalid Email Format");
                ShowNotification("REGISTRATION ERROR", "Please Enter a Valid Email");
                yield break;
            }
            else if (!IsPasswordValid(password))
            {
                Debug.LogError("Password Does Not Meet The Requirements.");
                ShowNotification("REGISTRATION ERROR", "Password Does Not Meet The Requirements");
                yield break;
            }

            // Prepare user data for saving
            UserData newUser = new UserData(name, email, password, parsedAge);

            // Convert user data to JSON
            string json = JsonUtility.ToJson(newUser);

            // Save user data to Firebase Realtime Database
            var setTask = databaseReference
                .Child("users")
                .Child(email.Replace(".", "_"))
                .SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => setTask.IsCompleted);

            // Check for errors during saving to the database
            if (setTask.Exception != null)
            {
                Debug.LogError("Failed to save user data: " + setTask.Exception);
                ShowNotification("REGISTRATION ERROR", "Failed to save data.");
            }
            else
            {
                Debug.Log("User registered successfully.");
            }
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
                return false;
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;
            if (!Regex.IsMatch(password, @"[0-9]"))
                return false;
            if (!Regex.IsMatch(password, @"[\W_]"))
                return false;
            return true;
        }

        private bool IsValidEmail(string email)
        {
            string pattern =
                @"^[a-zA-Z0-9._%+-]+@(gmail\.com|outlook\.com|yahoo\.com|hotmail\.com)$";
            return Regex.IsMatch(email, pattern);
        }

        private void OnPasswordChanged(string password)
        {
            int strength = EvaluatePasswordStrength(password);

            Color weakColor = new Color(128f / 255f, 16f / 255f, 4f / 255f); // Weak color
            Color mediumColor = new Color(224f / 255f, 152f / 255f, 56f / 255f); // Medium color
            Color strongColor = new Color(43f / 255f, 159f / 255f, 15f / 255f); // Strong color

            if (passwordStrengthText != null)
            {
                switch (strength)
                {
                    case 5:
                        passwordStrengthText.text = "Strong";
                        passwordStrengthText.color = strongColor;
                        break;
                    case 3:
                    case 4:
                        passwordStrengthText.text = "Medium";
                        passwordStrengthText.color = mediumColor;
                        break;
                    case 1:
                    case 2:
                        passwordStrengthText.text = "Weak";
                        passwordStrengthText.color = weakColor;
                        break;
                    default:
                        passwordStrengthText.text = "";
                        break;
                }
            }
        }

        private int EvaluatePasswordStrength(string password)
        {
            int strength = 0;

            if (password.Length >= 8)
                strength++;
            if (Regex.IsMatch(password, "[A-Z]"))
                strength++;
            if (Regex.IsMatch(password, "[a-z]"))
                strength++;
            if (Regex.IsMatch(password, "[0-9]"))
                strength++;
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
                strength++;

            return strength;
        }

        public void ResetPasswordSubmit()
        {
            string resetPasswordEmail = passwordResetField.text;

            if (string.IsNullOrEmpty(resetPasswordEmail))
            {
                Debug.LogError("Reset Password Email Field Is Empty");
                ShowNotification("Reset Password Error", "Email Field Is Required");
                return;
            }

            auth.SendPasswordResetEmailAsync(resetPasswordEmail)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("Send Password Reset Email Async Was Canceled");
                        ShowNotification("Reset Password Error", "Request Was Canceled");
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.LogError(
                            "Send Password Reset Email Async Encountered An Error: "
                                + task.Exception
                        );
                        ShowNotification(
                            "Reset Password Error",
                            "Failed To Send Password Reset Email"
                        );
                        return;
                    }

                    Debug.Log("Password Reset Email Sent Successfully");
                    ShowNotification(
                        "Reset Password Success",
                        "An Email Has Been Sent For Password Reset"
                    );
                });
        }

        private void ShowNotification(string title, string message)
        {
            if (notifTitleField != null && notifMessageField != null)
            {
                notifTitleField.text = title;
                notifMessageField.text = message;
            }
        }
    }

    [System.Serializable]
    public class UserData
    {
        public string userName;
        public string email;
        public string password;
        public int age;

        public UserData(string userName, string email, string password, int parsedAge)
        {
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.age = parsedAge;
        }
    }
}
