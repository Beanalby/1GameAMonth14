using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FlippyMusic : MonoBehaviour {
    private static FlippyMusic _instance = null;

    private int pos;
    public void Awake() {
        if(_instance != null) {
            Destroy(gameObject);
        } else {
            _instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    public void OnLevelWasLoaded() {
        audio.timeSamples = pos;
        audio.Play();
    }

    public void PlayerDied() {
        pos = audio.timeSamples;
        audio.Stop();
    }
    
}
