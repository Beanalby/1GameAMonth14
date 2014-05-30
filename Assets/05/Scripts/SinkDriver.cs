using UnityEngine;
using System.Collections;
using System;

public class SinkDriver : MonoBehaviour {

    private static SinkDriver _instance = null;
    public static SinkDriver instance {
        get { return _instance; }
    }

    public string[] stages;

    public void Start() {
        if(_instance != null) {
            // there's already a sinkDriver, die!
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void HoleComplete() {
        StartCoroutine(holeComplete());
    }

    private IEnumerator holeComplete() {
        string nextStage = stages[Array.IndexOf(stages, Application.loadedLevelName) + 1];
        yield return new WaitForSeconds(3);
        Debug.Log("Loading [" + nextStage + "]");
    }

}
