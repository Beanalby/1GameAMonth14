using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    public void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        GetComponent<AudioSource>().timeSamples = pos;
        GetComponent<AudioSource>().Play();
    }

    public void PlayerDied() {
        pos = GetComponent<AudioSource>().timeSamples;
        GetComponent<AudioSource>().Stop();
    }
    
}
