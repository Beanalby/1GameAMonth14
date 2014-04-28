using UnityEngine;
using System.Collections;

public class gbPart : MonoBehaviour {
    public void PickedUp() {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        Destroy(gameObject);
    }
}
