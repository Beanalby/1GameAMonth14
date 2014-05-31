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
        int index = Array.IndexOf(stages, Application.loadedLevelName);
        if (index == -1) {
            Debug.LogError("stage [" + Application.loadedLevelName + "] not found in list of stages");
            yield break;
        }
        string nextStage = stages[index + 1];
        yield return new WaitForSeconds(3);
        Application.LoadLevel(nextStage);
    }

}
