using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleDriver : MonoBehaviour {
    public GUISkin skin;
    public Texture2D titleImage;

    public void Update() {
        if (Input.GetButtonDown("Jump")) {
            SceneManager.LoadScene("Stage1");
        }
    }
    public void OnGUI() {
        GUI.skin = skin;
        GUI.DrawTexture(
            new Rect( Screen.width / 2 - titleImage.width / 2,
                Screen.height / 2 - titleImage.height / 2,
                titleImage.width, titleImage.height),
            titleImage);
        GUI.Label(new Rect(0, 300, Screen.width, Screen.height - 300),
            "Press space to begin");
    }
}
