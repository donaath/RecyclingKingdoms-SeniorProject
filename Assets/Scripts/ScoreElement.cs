using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CharacterSelector.Scripts
{
    public class ScoreElement : MonoBehaviour
    {
        public TMP_Text usernameText;
        public TMP_Text moodText;
        public TMP_Text xpText;

        public void NewScoreElement(string _username, int _mood, int _xp)
        {
            usernameText.text = _username;
            moodText.text = _mood.ToString();
            xpText.text = _xp.ToString();
        }
    }
}
