using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlippyIntro : MonoBehaviour {

    public GUISkin skin;
    public Texture2D title, introBg;

    GUIStyle introStyle;

    public void Start() {
        introStyle = new GUIStyle(skin.label);
        introStyle.alignment = TextAnchor.MiddleCenter;
    }
    public void Update() {
        if(Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("game");
        }
    }

    public void OnGUI() {
        Rect titleRect = new Rect(Screen.width / 2 - title.width / 2,
            25, title.width, title.height);
        Rect introRect = new Rect(Screen.width / 2 - introBg.width,
            Screen.height / 2 + 75, introBg.width * 2, introBg.height * 2);

        GUI.DrawTexture(titleRect, title);
        GUI.DrawTexture(introRect, introBg);
        GUI.Label(introRect, "Click or space\nto jump", introStyle);
    }
}
