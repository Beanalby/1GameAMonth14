using UnityEngine;
using System.Collections;

public class SwayMusic : MonoBehaviour {
    public void Awake() {
        GameObject.DontDestroyOnLoad(gameObject);
    }
}
