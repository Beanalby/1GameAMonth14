using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class HoleInfo {
    public string name;
    public int par;
}

public class SinkDriver : MonoBehaviour {

    private static SinkDriver _instance = null;
    public static SinkDriver instance {
        get { return _instance; }
    }

    public GUISkin skin;
    public HoleInfo[] stages;

    private int padding = 10;
    private int textHeight = 100;
    private int textWidth = 300;

    private int totalStrokes = 0;
    public int TotalStrokes {
        get { return totalStrokes; }
    }
    private int strokesThisHole = 0;
    private List<int> scores = new List<int>();
    public List<int> Scores {
        get { return scores; }
    }
    private GUIStyle labelCurrent, labelTotal, labelHole;
    private float thisPar;
    public void Awake() {
        if(_instance != null) {
            // there's already a sinkDriver, die!
            Destroy(gameObject);
            return;
        }
        _instance = this;
        GameObject.DontDestroyOnLoad(gameObject);

        labelHole = new GUIStyle(skin.label);
        labelHole.alignment = TextAnchor.UpperRight;
        labelCurrent = new GUIStyle(skin.label);
        labelCurrent.alignment = TextAnchor.LowerLeft;
        labelTotal = new GUIStyle(skin.label);
        labelTotal.alignment = TextAnchor.LowerRight;
        InitHole();
    }

    public void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoaded;
    }
    public void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoaded;
    }
    void OnLevelFinishedLoaded(Scene scene, LoadSceneMode mode) {
        scores.Add(strokesThisHole);
        InitHole();
    }

    private void InitHole() {
        strokesThisHole = 0;
        for (int i = 0; i < stages.Length; i++) {
            if (stages[i].name == SceneManager.GetActiveScene().name) {
                thisPar = stages[i].par;
            }
        }
    }

    public void OnGUI() {
        if (SceneManager.GetActiveScene().name == "summary") {
            return;
        }
        Vector3 shadowDir = new Vector3(-2, 2, 0);
        GUIContent content = new GUIContent();
        Rect holeRect = new Rect(Screen.width - (padding + textWidth), padding, textWidth, textHeight);
        Rect currentRect = new Rect(padding, Screen.height - (padding + textHeight), textWidth, textHeight);
        Rect totalRect = new Rect(Screen.width - (padding + textWidth), Screen.height - (padding + textHeight), textWidth, textHeight);

        content.text = SceneManager.GetActiveScene().name + ", Par " + thisPar;
        ShadowAndOutline.DrawShadow(holeRect, content, labelHole, Color.white, Color.black, shadowDir);
        content.text = "Strokes: " + strokesThisHole;
        ShadowAndOutline.DrawShadow(currentRect, content, labelCurrent, Color.white, Color.black, shadowDir);
        content.text = "Total: " + totalStrokes;
        ShadowAndOutline.DrawShadow(totalRect, content, labelTotal, Color.white, Color.black, shadowDir);
    }

    public void Stroked() {
        strokesThisHole++;
        totalStrokes++;
    }

    public void HoleComplete() {
        StartCoroutine(holeComplete());
    }

    private IEnumerator holeComplete() {
        int index;
        bool found=false;
        for (index = 0; index < stages.Length; index++) {
            if (stages[index].name == SceneManager.GetActiveScene().name) {
                found = true;
                break;
            }
        }
        string nextStage;
        if (!found || index >= stages.Length-1) {
            nextStage = "summary";
        } else {
            nextStage = stages[index + 1].name;
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(nextStage);
    }

}
