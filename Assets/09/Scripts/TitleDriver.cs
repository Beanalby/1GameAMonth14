using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class TitleDriver : MonoBehaviour {

        public GUISkin skin;

        private string title = "Coins Coins Coins";
        private string desc = "Collect coins before time runs out!";
        private string begin = "Press spacebar to begin";

        private GUIStyle titleStyle, descStyle, beginStyle;

        public void Start() {
            titleStyle = new GUIStyle(skin.label);
            titleStyle.fontSize *= 2;
            titleStyle.alignment = TextAnchor.UpperCenter;
            descStyle = new GUIStyle(skin.label);
            descStyle.alignment = TextAnchor.UpperCenter;
            beginStyle = new GUIStyle(skin.label);
            beginStyle.fontSize = (int)(beginStyle.fontSize * 1.5f);
            beginStyle.alignment = TextAnchor.UpperCenter;
        }

        public void Update() {
            if(Input.GetButtonDown("Jump")) {
                Application.LoadLevel("game");
            }
        }

        public void OnGUI() {
            Rect titleRect = new Rect(0, 75, Screen.width, 100);
            Rect descRect = new Rect(200, 250, Screen.width - 400, 200);
            Rect beginRect = new Rect(0, 380, Screen.width, 200);

            ShadowAndOutline.DrawShadow(titleRect, new GUIContent(title),
                titleStyle, Color.white, Color.black, new Vector2(-6, -6));
            ShadowAndOutline.DrawShadow(descRect, new GUIContent(desc),
                descStyle, Color.white, Color.black, new Vector2(-6, -6));
            ShadowAndOutline.DrawShadow(beginRect, new GUIContent(begin),
                beginStyle, Color.white, Color.black, new Vector2(-6, -6));
        }
    }
}