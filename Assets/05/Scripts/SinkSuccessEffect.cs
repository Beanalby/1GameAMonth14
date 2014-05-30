using UnityEngine;
using System.Collections;

public class SinkSuccessEffect : MonoBehaviour {
    public Texture img;

    public void OnGUI() {
        GUI.DrawTexture(new Rect(
            Screen.width / 2 - (img.width / 2), 10,
            img.width, img.height), img);
    }
}
