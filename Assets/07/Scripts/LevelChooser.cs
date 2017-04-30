using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace onegam_1407 {
    public class LevelChooser : MonoBehaviour {
        public Texture background;
        public Texture selection;

        int currentSelection = 0;

        public void Update() {
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                currentSelection = -1;
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                currentSelection = 1;
            }
            if(Input.GetKeyDown(KeyCode.Space) && currentSelection != 0) {
                if(currentSelection == -1) {
                    SceneManager.LoadScene("stage1");
                } else {
                    SceneManager.LoadScene("stage2");
                }
            }
        }

        public void OnGUI() {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),
                background);
            if(currentSelection == -1) {
                GUI.DrawTexture(new Rect(22, 215, selection.width, selection.height),
                    selection);
            }
            if(currentSelection == 1) {
                GUI.DrawTexture(new Rect(482, 215, selection.width, selection.height),
                    selection);
            }
        }
    }
}