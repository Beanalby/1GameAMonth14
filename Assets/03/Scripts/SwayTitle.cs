using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwayTitle : MonoBehaviour {

    public void Update() {
        if(Input.GetButtonDown("Jump")) {
            SceneManager.LoadScene("stage1");
        }
    }
}
