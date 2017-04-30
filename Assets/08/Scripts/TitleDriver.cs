using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace onegam_1408 {
    public class TitleDriver : MonoBehaviour {
        public GUISkin skin;

        public void OnGUI() {
            GUI.skin = skin;
            Rect buttonRect = new Rect(Screen.width / 2 - 200, Screen.height * .8f, 400, 50);
            if(GUI.Button(buttonRect, "Press Space to start") || Input.GetButtonDown("Jump")) {
                SceneManager.LoadScene("Game");
            }
        }
    }
}