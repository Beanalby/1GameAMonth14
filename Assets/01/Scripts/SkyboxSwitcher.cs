using UnityEngine;
using System.Collections;

public class SkyboxSwitcher : MonoBehaviour {
    public Material human, race, dino, space;

    public void Start() {
        RenderSettings.skybox = human;
    }

    public void UpdateWorld(GameWorld world) {
        Material box = null;
        switch (world) {
            case GameWorld.human:
                box = human; break;
            case GameWorld.race:
                box = race; break;
            case GameWorld.dino:
                box = dino; break;
            case GameWorld.space:
                box = space; break;
        }
        RenderSettings.skybox = box;
    }
}
