using UnityEngine;
using System.Collections;

public class TerrainSwitcher : MonoBehaviour {
    public GameObject human, race, dino, space;

    private GameObject current;

    public void Start() {
        current = human;
        human.SetActive(true);
    }

    public void UpdateWorld(GameWorld world) {
        if (current) {
            current.SetActive(false);
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
        if (current) {
            current.SetActive(true);
        }
    }
}
