using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepfatherScript : MonoBehaviour
{
    public GameObject childSpot;
    public GameObject lastObj;

    public float MinspdChase = 5;
    public float spdChase = 5;

    private float DistancePlayerFather;
    public float DistanceToReact=3;
    public Vector2 lastSeen;

    private Vector3 GoTo;
    private Vector3 GoToObj;

    public GameObject gm ;
    public bool iGoTo = false;
    public bool iGoAct = false;
    public bool Checker = true;
    private bool pauseChase = false;
    public float SpeedCap = 3;

    public List<Vector2> rondoPoints = new List<Vector2>(); 
    public bool onRound = true;
    private int roundPlace = 0;
    public bool lookinAt = true;
    public bool notAvailable = false;
    private Animator Anim;

    public float DistToActivObj;
    private bool ActivObj;
    public bool Scream;
    private bool FirstDoor = false;
    float step;
    public float DistanceOfRound = 10;

    public AudioSource MySound;
    public List<AudioClip> Walk;
    int tracklistSFX = 0;
    bool CwalkOnPlay = false;
    // Start is called before the first frame update
    void Start()
    {
        Anim = this.GetComponent<Animator>();
        GameController.Instance.StepFather = this;
        MinspdChase = spdChase;
        rondoPoints.Add(new Vector2(transform.position.x+DistanceOfRound,transform.position.y));
        rondoPoints.Add(new Vector2(transform.position.x-DistanceOfRound,transform.position.y));
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameController.Instance.FirstCry)
            return;
        
        /* 
        if (iGoTo) {
            onRound = false;
            if (this.transform.position != GoTo) {
                float step = spdChase * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position,new Vector2(GoTo.x,transform.position.y) , step);
                Anim.SetBool("Walk",true);
                if(transform.position.x >GoTo.x){
                    transform.localScale = new Vector3(-1,1,1);
                }else{
                    transform.localScale = new Vector3(1,1,1);
                }
                if( != null){
                    Destroy();
                }
            }
            else {
                rondoPoints.Clear();

                if (lookinAt) {
                    Debug.Log("Je partir à droate");
                    rondoPoints.Add(new Vector2(GoTo.x+DistanceOfRound,GoTo.y));
                    rondoPoints.Add(new Vector2(GoTo.x-DistanceOfRound,GoTo.y));
                }
                else {
                    Debug.Log("Je partir à gôche");
                    rondoPoints.Add(new Vector2(GoTo.x-DistanceOfRound,GoTo.y));
                    rondoPoints.Add(new Vector2(GoTo.x+DistanceOfRound,GoTo.y));
                }
                roundPlace = 0;
                iGoTo = false;
                onRound = true;
            }
        }else if (iGoAct) {
            if (transform.position.x != GoToObj.x) {
                float step = spdChase * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(GoToObj.x,transform.position.y), step);
                Anim.SetBool("Walk",true);
                if(transform.position.x >GoToObj.x){
                    transform.localScale = new Vector3(-1,1,1);
                }else{
                    transform.localScale = new Vector3(1,1,1);
                }
            }
            else {
                iGoAct = false;
                StartCoroutine(Activate());
            }
        }else if (onRound) {
            if (this.transform.position.x != rondoPoints[roundPlace].x ) {
                float step = spdChase * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position,new Vector2(rondoPoints[roundPlace].x,transform.position.y) , step);
                Anim.SetBool("Walk",true);
                if(transform.position.x >rondoPoints[roundPlace].x){
                    transform.localScale = new Vector3(-1,1,1);
                }else{
                    transform.localScale = new Vector3(1,1,1);
                }
            }
            else {
                roundPlace ++;
                if (roundPlace == rondoPoints.Count) {
                    Debug.Log("Je m'avance en arrière");
                    roundPlace = 0;
                }
            }
        }*/
        if(GoTo != Vector3.zero){
            step = spdChase * Time.deltaTime;
            Anim.SetBool("Walk",true);
            float DistToObj = Vector2.Distance(this.transform.position,new Vector2(GoTo.x,transform.position.y));
            if(DistToActivObj < DistToObj){
                if(CwalkOnPlay){
                    CwalkOnPlay = true;
                    StartCoroutine(DFootsteps());
                }
                transform.position = Vector2.MoveTowards(transform.position,new Vector2(GoTo.x,transform.position.y) , step);
            }
        }
        if (this.transform.position.x < GoTo.x){
            transform.localScale = new Vector3(1,1,1);
            lookinAt = true;
        } 
        else{
            transform.localScale = new Vector3(-1,1,1);
            lookinAt = false;
        }
        
        if(ActivObj){
            float DistToObj = Vector2.Distance(this.transform.position,new Vector2(GoTo.x,transform.position.y));
            if(DistToActivObj >= DistToObj){
                GoTo = Vector3.zero;
                StartCoroutine(Activate());
            }
        }
        if(lastSeen != Vector2.zero){
            float DistToObj = Vector2.Distance(this.transform.position,new Vector2(GoTo.x,transform.position.y));
            if(DistToActivObj >= DistToObj){
                if (lookinAt){
                    GoTo = rondoPoints[1];
                } 
                else{
                    GoTo = rondoPoints[0];
                }
            }
        }

        if (Checker) {
            this.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        }
        else if (!Checker) {
            this.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        }
        DistanceTrigger ();
        // Liste des rondes ici       
    }
    IEnumerator DFootsteps() {
        MySound.PlayOneShot(Walk[tracklistSFX]);
        yield return new WaitForSeconds(0.5f);
        tracklistSFX ++;
        if (tracklistSFX >= 2) {
            tracklistSFX = 0;
        }
        CwalkOnPlay = false;
    }

    public void ChildScream(){
        lastSeen = GameController.Instance.PlayerScript.transform.position;
        Scream = true;
    }
    public void GoChild()
    {
        onRound = false;
        GoTo = new Vector2(GameController.Instance.ScriptPlayer.transform.position.x,transform.position.y);
        iGoTo = true;
        lastSeen = Vector2.zero;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.GetComponent<InteractionObject>() != null && !other.GetComponent<InteractionObject>().Active && (GoTo == Vector3.zero || Scream) && !ActivObj) {
            iGoTo = false;
            notAvailable = true;
            lastObj = other.gameObject;
            GoTo = other.transform.position;
            iGoAct = true;
            pauseChase = true;
            ActivObj = true;
            Debug.Log("Je vais ouvrir une porte");
        }else if (other.GetComponent<InteractionObject>() != null && !other.GetComponent<InteractionObject>().Active  && (GoTo == Vector3.zero || Scream) && !ActivObj) {
            lastObj = other.gameObject;
            notAvailable = true;
            GoTo = lastObj.transform.position;
            iGoAct = true;
            Checker = false;
            ActivObj = true;
            Debug.Log("Je vais ouvrir un switch");
        }
    }

    private void DistanceTrigger () {
        DistancePlayerFather = Vector2.Distance(transform.position,GameController.Instance.PlayerScript.transform.position);
        if(DistancePlayerFather <= DistanceToReact && !GameController.Instance.ScriptPlayer.GetHidden()){
            if(!ActivObj && FirstDoor){
                SoundManager.Instance.ISpotYou();
                GoChild();
                spdChase = SpeedCap;
                Scream = false;
            }
            GameController.Instance.FirstCry = true;
            GameController.Instance.SpeedBoost();
            
        }
        else {
            if(lastSeen == Vector2.zero && !Scream && !ActivObj){
                Scream = true;
                lastSeen = GameController.Instance.PlayerScript.transform.position;
                GameController.Instance.ReduceBoost();
                WhatUDo();
            }
        }
    }
    private void OnTriggerExit2D (Collider2D other) {
    }

    private void WhatUDo(){
        rondoPoints.Clear();
        rondoPoints.Add(new Vector2(lastSeen.x+DistanceOfRound,transform.position.y));
        rondoPoints.Add(new Vector2(lastSeen.x-DistanceOfRound,transform.position.y));
        if (lookinAt){
            GoTo = rondoPoints[0];
        } 
        else{
            GoTo = rondoPoints[1];
        }
    }

    public IEnumerator Activate() {
        InteractionObject objScript = lastObj.GetComponent<InteractionObject>();
        Anim.SetBool("Activate",true);
        FirstDoor = true;
        yield return new WaitForSeconds(0.7f);
        ActivObj = false;
        objScript.objActive();
        lastObj = null;
        Anim.SetBool("Activate",false);
        WhatUDo();
    }
}
