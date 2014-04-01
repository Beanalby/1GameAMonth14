using UnityEngine;
using System.Collections;

public class SwayCliffhanger : MonoBehaviour {

    public GUISkin skin;

    public void OnGUI() {
        GUI.skin = skin;
        GUI.Label(new Rect(0, 100, Screen.width, 200), "To be continued...");
    }
}
