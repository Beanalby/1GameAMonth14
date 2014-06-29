using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour {

    private float enabledAlpha = 1f;
    private float disabledAlpha = .5f;

    private Material mat;
    Color color;

    public void Start() {
        mat = GetComponent<MeshRenderer>().material;
        color = mat.color;
        SwingDisabled();
    }
    public void SwingDisabled() {
        color.a = disabledAlpha;
        mat.color = color;
    }

    public void SwingEnabled() {
        color.a = enabledAlpha;
        mat.color = color;
    }


}
