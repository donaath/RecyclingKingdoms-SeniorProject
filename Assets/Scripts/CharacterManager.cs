using System.Collections;
using System.Collections.Generic;
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

        protected override void Init()
        {
            Persist = true;
            base.Init();
        }

        public void Start()
        {
            if (SpawnPoint != null)
            {
                SetCurrentCharacterType(_currentIndex);
            }
        }

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

        public void CreateCurrentCharacter(string name)
        {
            _currentCharacter = Instantiate<CharacterInfo>(
                _currentCharacterType,
                SpawnPoint.transform.position,
                Quaternion.identity
            );

            _currentCharacter.gameObject.SetActive(false);
            _currentCharacter.Name = name;

            DontDestroyOnLoad(_currentCharacter);
        }

        public CharacterInfo GetCurrentCharacter()
        {
            return _currentCharacter;
        }
    }
}
