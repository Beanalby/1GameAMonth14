using UnityEngine;
using System.Collections;

public class FlippyDriver : MonoBehaviour {

    public GUISkin skin;
    public Texture2D scoreBg;
    private int score = 0;
    GUIStyle scoreLabel, scoreValue;

    public void Start() {
        scoreLabel = new GUIStyle(skin.label);
        scoreLabel.alignment = TextAnchor.MiddleLeft;
        scoreValue = new GUIStyle(skin.label);
        scoreValue.alignment = TextAnchor.MiddleRight;
    }
    public void PipePassed(FlippyPipes pipes) {
        score++;
    }

    public void OnGUI() {
        GUI.skin = skin;
        Rect scoreRect = new Rect(10, 10, scoreBg.width, scoreBg.height);
        GUI.DrawTexture(scoreRect, scoreBg);
        int padding = 10;
        scoreRect.xMin += padding;
        scoreRect.xMax -= padding;
        scoreRect.yMin += padding;
        scoreRect.yMax -= padding;
        GUI.Label(scoreRect, "Score: ", scoreLabel);
        GUI.Label(scoreRect, score.ToString(), scoreValue);
    }
}
