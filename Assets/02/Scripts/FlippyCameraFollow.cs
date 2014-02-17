using UnityEngine;
using System.Collections;

public class FlippyCameraFollow : MonoBehaviour {
    public GameObject target;

    public void Update() {
        Vector3 pos = transform.position;
        pos.x = target.transform.position.x;
        transform.position = pos;
    }
}
