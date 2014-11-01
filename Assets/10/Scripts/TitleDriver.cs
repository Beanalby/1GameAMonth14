using UnityEngine;
using System.Collections;

namespace onegam_1410 {
    public class TitleDriver : MonoBehaviour {
        public GUISkin skin;
        
        GUIContent titleContent, msgContent, beginContent;
        GUIStyle titleStyle, msgStyle, beginStyle;
        public void Start() {
            titleStyle = new GUIStyle(skin.label);
            titleStyle.alignment = TextAnchor.UpperCenter;
            msgStyle = new GUIStyle(skin.label);
            msgStyle.fontSize = msgStyle.fontSize / 2;
            msgStyle.alignment = TextAnchor.UpperCenter;
            beginStyle = new GUIStyle(skin.label);
            beginStyle.fontSize = beginStyle.fontSize / 2;
            beginStyle.alignment = TextAnchor.UpperCenter;

            titleContent = new GUIContent("CUBEAGEDDON!");
            msgContent = new GUIContent("Drive around with arrow keys to knock over as many cubes as you can!");
            beginContent = new GUIContent("Press Space to begin");
        }

        public void Update() {
            if(Input.GetButtonDown("Jump")) {
                Application.LoadLevel("game");
            }
        }
        public void OnGUI() {
            Rect titleRect = new Rect(0, 25, Screen.width, 300);
            Rect msgRect = new Rect(0, 200, Screen.width, 300);
            Rect beginRect = new Rect(0, 400, Screen.width, 300);

            ShadowAndOutline.DrawShadow(titleRect, titleContent, titleStyle,
                Color.white, Color.black, new Vector2(3, -3));
            ShadowAndOutline.DrawShadow(msgRect, msgContent, msgStyle,
                Color.white, Color.black, new Vector2(2, -2));
            ShadowAndOutline.DrawShadow(beginRect, beginContent, beginStyle,
                Color.white, Color.black, new Vector2(3, -3));
        }
    }
}