using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace onegam_1406 {
    public class TitleDriver : MonoBehaviour {
        public Texture2D title, lower;

        public void OnGUI() {
            GUI.DrawTexture(new Rect(
                (Screen.width / 2) - title.width / 2, 0,
                title.width, title.height),
                title);
            GUI.DrawTexture(new Rect(
                (Screen.width / 2) - lower.width / 2,
                Screen.height - (10 + lower.height),
                lower.width, lower.height),
                lower);
        }

        public void Update() {
            if(Input.GetButtonDown("Fire1")) {
                SceneManager.LoadScene("game");
            }
        }
    }
}