using UnityEngine;
using System.Collections;

public class SwayTitle : MonoBehaviour {

    public void Update() {
        if(Input.GetButtonDown("Jump")) {
            Application.LoadLevel("stage1");
        }
    }
}
