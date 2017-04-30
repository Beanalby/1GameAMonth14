using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SinkTitle : MonoBehaviour {
    public SinkBall ball;
    public Texture titleImage;

    public void Start() {
        ball.IsControllable = false;
        StartCoroutine(HitBall());
    }
    IEnumerator HitBall() {
        yield return new WaitForSeconds(2);
        ball.ApplyHit(new Vector3(22, 0, 0));
    }
    public void OnGUI() {
        GUI.DrawTexture(new Rect(Screen.width / 2 - (titleImage.width / 2), 5, titleImage.width, titleImage.height), titleImage);
        Rect buttonRect = new Rect(Screen.width * .3f, Screen.height * .7f, Screen.width * .4f, Screen.height * .2f);
        if (GUI.Button(buttonRect, "Click to Start")) {
            SceneManager.LoadScene("hole 1");
        }
    }
}
