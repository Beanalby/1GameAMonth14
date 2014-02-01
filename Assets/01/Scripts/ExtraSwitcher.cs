using UnityEngine;
using System.Collections;

public class ExtraSwitcher : MonoBehaviour {
    public GameObject[] human, race, dino, space;

    public GameObject[] current;

    public void Start() {
        current = human;
        foreach (GameObject obj in current) {
            obj.SetActive(true);
        }
    }

    public void UpdateWorld(GameWorld world) {
        foreach (GameObject obj in current) {
            obj.SetActive(false);
        }
        switch (world) {
            case GameWorld.human:
                current = human; break;
            case GameWorld.race:
                current = race; break;
            case GameWorld.dino:
                current = dino; break;
            case GameWorld.space:
                current = space; break;
        }
        foreach (GameObject obj in current) {
            obj.SetActive(true);
        }
    }
}
