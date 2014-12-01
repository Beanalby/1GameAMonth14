using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    public class TitleDriver : MonoBehaviour {
        public Texture2D titleImage;

        public void Update() {
            if(Input.GetButtonDown("Jump")) {
                Application.LoadLevel("11game");
            }
        }
        public void OnGUI() {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), titleImage);
        }
    }
}
