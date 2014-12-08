using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class GameDriver : MonoBehaviour {

        public GUISkin skin;
        private GUIStyle sickStyle, scoreStyle;
        private int score = 0;

        public void Start() {
            sickStyle = new GUIStyle(skin.label);
            sickStyle.alignment = TextAnchor.UpperLeft;
            scoreStyle = new GUIStyle(skin.label);
            scoreStyle.alignment = TextAnchor.UpperRight;
        }

        public void OnGUI() {
            GUI.skin = skin;
            Rect sickRect = new Rect(10, 10, 200, 50);
            Rect scoreRect = new Rect(Screen.width - 210, 10, 200, 50);
            GUIContent content;

            content = new GUIContent("Sickness: " + Player.Instance.Sickness.ToString("N0"));
            ShadowAndOutline.DrawShadow(sickRect, content, sickStyle,
                Color.white, Color.black, new Vector2(-3, -3));

            content = new GUIContent("Score: " + score);
            ShadowAndOutline.DrawShadow(scoreRect, content, scoreStyle,
                Color.white, Color.black, new Vector2(-3, -3));

        }
    }
}