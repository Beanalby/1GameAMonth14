using UnityEngine;
using System.Collections;

public class FlippyProgress : MonoBehaviour {

    public Texture meter;
    public Texture marker;

    private int progressStart = 230;
    private int progressEnd = 440;
    private int markerHeight = 20;

    private FlippyDriver driver;
    private FlippyPlayer player;

    float percent;

    public void Start() {
        driver = GameObject.FindObjectOfType<FlippyDriver>();
        player = GameObject.FindObjectOfType<FlippyPlayer>();
    }
    public void Update() {
        if(player.IsPlaying) {
            // only update while we're playing
            percent = driver.StageProgress;
        }
    }
    public void OnGUI() {
        float x = Mathf.Lerp(progressStart, progressEnd, percent) - marker.width/2;
        Rect meterRect = new Rect(progressStart, 10, progressEnd - progressStart, 10);
        Rect markerRect = new Rect(x, markerHeight, marker.width, marker.height);
        GUI.DrawTexture(meterRect, meter);
        GUI.DrawTexture(markerRect, marker);

    }
}
