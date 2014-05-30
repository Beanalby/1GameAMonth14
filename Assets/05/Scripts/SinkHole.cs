using UnityEngine;
using System.Collections;

public class SinkHole : MonoBehaviour {
    /// <summary>
    /// Scale up gravity so the 1M-diameter ball feels smaller
    /// </summary>
    public float gravityScale = 10;

    public GameObject successPrefab;

    public void Start() {
        Physics.gravity *= gravityScale;
    }

    public void HoleComplete() {
        GameObject obj = (GameObject) Instantiate(successPrefab);
        obj.transform.position = transform.position;
    }
}
