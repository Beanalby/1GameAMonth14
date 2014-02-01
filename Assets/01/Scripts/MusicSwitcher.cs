using UnityEngine;
using System.Collections;

public class MusicSwitcher : MonoBehaviour {
    public AudioSource intro, human, race, dino, space;

    private AudioSource current, newSource;
    private float mergeStart=-1, mergeDuration = 1f;

    public void Start() {
        current = intro;
        //intro.timeSamples = 305000;
    }
    public void Update() {
        //Debug.Log("Samples: " + current.timeSamples
        //        + ", isPlaying=" + current.isPlaying 
        //        + " playing: " + current.clip.name);
        if (intro.isPlaying && !human.isPlaying && intro.timeSamples >= 385000) {
            current = human;
            human.Play();
            //human.PlayDelayed(100);
        }
        if(mergeStart != -1) {
            float percent = (Time.time - mergeStart) / mergeDuration;
            //Debug.Log("From " + current.name + " to " + newSource.name + ", percent=" + percent);
            if (percent >= 1) {
                current.Stop();
                current = newSource;
                current.volume = 1;
                mergeStart = -1;
            } else {
                current.volume = 1 - percent;
                newSource.volume = percent;
            }
        }
    }

    public void UpdateWorld(GameWorld world) {
        AudioSource nextSource = null;
        switch (world) {
            case GameWorld.human:
                nextSource = human; break;
            case GameWorld.race:
                nextSource = race; break;
            case GameWorld.dino:
                nextSource = dino; break;
            case GameWorld.space:
                nextSource = space; break;
        }
        newSource = nextSource;
        newSource.volume = 0;
        current.volume = 1;
        newSource.timeSamples = current.timeSamples;
        newSource.Play();
        mergeStart = Time.time;
    }
}
