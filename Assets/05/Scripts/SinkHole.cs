﻿using UnityEngine;
using System.Collections;

public class SinkHole : MonoBehaviour {
    
    /// <summary>
    /// Scale up gravity so the 1M-diameter ball feels smaller
    /// </summary>
    private float gravityScale = 10;

    public GameObject successPrefab;

    public void Start() {
        Physics.gravity *= gravityScale;
    }

    public void OnHit(SinkBall ball) {
        HoleComplete(ball);
    }
    public void HoleComplete(SinkBall ball) {
        ball.HoleComplete();
        SinkDriver.instance.HoleComplete();
        GameObject obj = (GameObject) Instantiate(successPrefab);
        obj.transform.position = transform.position;
    }
}
