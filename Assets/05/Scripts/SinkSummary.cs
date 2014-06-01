using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SinkSummary : MonoBehaviour {

    public GUISkin skin;

    private SinkDriver driver;
    private GUIStyle summaryStyle;
    private GUIContent summary;

    public void Start() {
        driver = SinkDriver.instance;
        StringBuilder sb = new StringBuilder();
        sb.Append("Results:\n");
        List<int> scores = driver.Scores;
        float totalPar = 0, totalResult=0;
        sb.Append("         Par Score Result\n");
        for (int i = 0; i < driver.stages.Length; i++) {
            int par = driver.stages[i].par;
            int score = scores[i];
            int result = score - par;
            sb.Append("Hole " + (i+1)
                + "   " + par.ToString().PadLeft(3)
                + "   " + scores[i].ToString().PadLeft(3)
                + "    " + result.ToString().PadLeft(3) + "\n");
            totalPar += par;
            totalResult += result;
        }
        sb.Append("----------\n");
        sb.Append(
            "Total:   " + totalPar.ToString().PadLeft(3)
            + "   " + driver.TotalStrokes.ToString().PadLeft(3)
            + "    " + totalResult.ToString().PadLeft(3) + "\n");
        
        summaryStyle = new GUIStyle(skin.label);
        summaryStyle.alignment = TextAnchor.MiddleLeft;
        summary = new GUIContent(sb.ToString());
    }
    public void OnGUI() {
        GUI.skin = skin;
        ShadowAndOutline.DrawShadow(new Rect(Screen.width * .2f, 0, Screen.width * .8f, Screen.height), summary, summaryStyle, Color.white, Color.black, new Vector2(-2, 2));
        Rect buttonRect = new Rect(Screen.width * .3f, Screen.height * .9f, Screen.width * .4f, Screen.height * .05f);
        if(GUI.Button(buttonRect, "Play Again")) {
            Destroy(driver.gameObject);
            Application.LoadLevel("hole 1");
        }

    }
}
