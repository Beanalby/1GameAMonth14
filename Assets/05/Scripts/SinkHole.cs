using UnityEngine;
using System.Collections;

public class SinkHole : MonoBehaviour {

    public bool showSuccess = true;

    /// <summary>
    /// Scale up gravity so the 1M-diameter ball feels smaller
    /// </summary>
    private float gravityScale = 2;

    public GameObject successPrefab;

    public void Start() {
        Physics.gravity = new Vector3(0, -9.8f, 0) * gravityScale;
    }

    public void OnHit(SinkBall ball) {
        HoleComplete(ball);
    }
    public void HoleComplete(SinkBall ball) {
        ball.HoleComplete();
        if (SinkDriver.instance) {
            SinkDriver.instance.HoleComplete();
        }
        if (showSuccess) {
            GameObject obj = (GameObject)Instantiate(successPrefab);
            obj.transform.position = transform.position;
        }
    }
}
