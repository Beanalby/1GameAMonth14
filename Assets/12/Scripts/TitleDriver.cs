using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace onegam_1412 {
    public class TitleDriver: MonoBehaviour {

        public GUISkin skin;

        private GUIStyle titleStyle, msgStyle;

        private GUIContent titleContent = new GUIContent("Food Poisoning is Fun!");
        private GUIContent msgContent = new GUIContent("I had food poisoning Ludum Dare weekened, now you can make others just as miserable.\n\nUse arrow keys to eat questionable food choices, then puke on people to share the suffering.");
        // Use this for initialization
        void Start() {
            titleStyle = new GUIStyle(skin.label);
            titleStyle.alignment = TextAnchor.UpperCenter;
            titleStyle.fontSize *= 2;
            msgStyle = new GUIStyle(skin.label);
            msgStyle.alignment = TextAnchor.UpperCenter;
        }

        public void OnGUI() {
            GUI.skin = skin;
            Rect titleRect = new Rect(0, 20, Screen.width, 100);
            Rect msgRect = new Rect(0, 210, Screen.width, 300);
            Rect startRect = new Rect(Screen.width/2 - 100, 400, 200, 100);

            ShadowAndOutline.DrawShadow(titleRect, titleContent, titleStyle,
                Color.white, Color.black, new Vector2(-3, -3));
            ShadowAndOutline.DrawShadow(msgRect, msgContent, msgStyle,
                Color.white, Color.black, new Vector2(-3, -3));
            if (GUI.Button(startRect, "Click to Start")) {
                SceneManager.LoadScene("game");
            }
        }
    }
}