using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class TitleDriver : MonoBehaviour {
        public Texture titleImage;

        public void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) {
                Application.LoadLevel("levelChooser");
            }
        }
        public void OnGUI() {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),
                titleImage);
        }

    }
}