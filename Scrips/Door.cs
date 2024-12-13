using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        void Start()
        {
            asource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (open)
            {
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);
            }
            else
            {
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * smooth);
            }
        }

        public void OpenDoor()
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
        }

        private void OnMouseDown()
        {
            OpenDoor();
            StartCoroutine(LoadNextScene());
        }

        private IEnumerator LoadNextScene()
        {
            // Wait for the door to fully open (adjust time as necessary)
            float doorOpenTime = 1.0f; // Time it takes for the door to open fully
            yield return new WaitForSeconds(doorOpenTime);

            // Wait for the sound to finish playing
            yield return new WaitForSeconds(asource.clip.length);

            // Now load the next scene
            SceneManager.LoadScene("PetStateV2");
        }
    }
}
