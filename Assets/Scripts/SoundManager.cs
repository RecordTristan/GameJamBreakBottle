﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
 
    public AudioSource myAudio;
    public AudioSource CreapySound;
    public AudioSource MusicGame;
    public AudioSource BackAncyety;
    public List<AudioClip> soundEffect = new List<AudioClip>();
    public bool childWalk = false;

    
    private bool CwalkOnPlay = false;
    private int tracklistSFX;
    /*
        0: Child steps 1
        1: Child steps 2
        2: Dad steps 1
        3: Dad steps
        4: Breathing
        5: Door
        6: Slow door
        7: Locked door
        8: Game Over
        9: Safe object
        10: TV sound
        11: Scream - 1
        12: Scream - 2
        13: Scream - 3
        14: Scream - 4
        15: Spotted
        16: unsafe-object
        17: Can't reach
        18: End
        19: Hide closet
        20: Hide couch
        21: Light switch on
    */
    private bool dadWalk = false;
    private bool DwalkOnPlay;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        myAudio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCreapySound();

    }

    void UpdateCreapySound(){
        CreapySound.volume = Mathf.Abs((GameController.Instance.ScriptPlayer.GetLightRange().magnitude)/(GameController.Instance.ScriptPlayer.MaxLightRange.magnitude)-1);
        MusicGame.volume = Mathf.Abs((GameController.Instance.ScriptPlayer.GetLightRange().magnitude)/(GameController.Instance.ScriptPlayer.MaxLightRange.magnitude));
    }
    
    IEnumerator CFootSteps() {
        myAudio.PlayOneShot(soundEffect[tracklistSFX]);
        yield return new WaitForSeconds(0.7f);
        tracklistSFX ++;
        if (tracklistSFX >= 2) {
            tracklistSFX = 0;
        }
        CwalkOnPlay = false;
    }

    IEnumerator DFootsteps() { //Rajouter l'activation de la variable dans le StepfatherScript
        int tracklist = 2;
        myAudio.PlayOneShot(soundEffect[tracklist]);
        yield return new WaitForSeconds(0.5f);
        tracklist ++;
        if (tracklist >= 4) {
            tracklist = 2;
        }
        DwalkOnPlay = false;
    }

    public void ScreamSong(){
        //myAudio.PlayOneShot(soundEffect[11]);
        int screamTrack = Random.Range(11, 15);
        myAudio.PlayOneShot(soundEffect[screamTrack]);
    }

    public void SafeSound(){
        myAudio.PlayOneShot(soundEffect[9]);
    }

    public void UnsafeSound() {
        myAudio.PlayOneShot(soundEffect[16]);
    }

    public void DoorOpen(){
        myAudio.PlayOneShot(soundEffect[5]);
    }

    public void GOSound() {
        myAudio.PlayOneShot(soundEffect[8]);
    }

    public void ISpotYou() {
        myAudio.PlayOneShot(soundEffect[15]);
    }
    public void DoorLook() {
        myAudio.PlayOneShot(soundEffect[7]);
    }

    public void CantReach() {
        myAudio.PlayOneShot(soundEffect[17]);
    }

    public void TheEnd() {
        myAudio.PlayOneShot(soundEffect[18]);
    }

    public void HideCloset() {
        myAudio.PlayOneShot(soundEffect[19]);
    }

    public void HideCouch() {
        myAudio.PlayOneShot(soundEffect[20]);
    }

    public void LightSwitch() {
        myAudio.PlayOneShot(soundEffect[21]);
    }
}
