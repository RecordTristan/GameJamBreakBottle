﻿using System.Collections;
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

    public GameObject gm;
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

    public float DistanceOfRound = 10;
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
        DistanceTrigger ();
        if(!GameController.Instance.FirstCry)
            return;
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
                if(gm != null){
                    Destroy(gm);
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
        }

        if (Checker) {
            this.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        }
        else if (!Checker) {
            this.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        }
        DistanceTrigger ();

        // Liste des rondes ici:

       
    }

    public void GoChild()
    {
        onRound = false;
        GoTo = new Vector2(GameController.Instance.ScriptPlayer.transform.position.x,transform.position.y);
        if (this.transform.position.x < GoTo.x)
            lookinAt = true;
        else
            lookinAt = false;
        iGoTo = true;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if(other.gameObject == gm){
            Debug.Log("Hello");
            iGoTo = true;
        }else{
            Debug.Log(Checker && other.GetComponent<InteractionObject>() != null);
            if (other.GetComponent<InteractionObject>() != null && iGoTo && !other.GetComponent<InteractionObject>().Active && !notAvailable) {
                iGoTo = false;
                notAvailable = true;
                lastObj = other.gameObject;
                GoToObj = other.transform.position;
                iGoAct = true;
                pauseChase = true;
                Debug.Log("Je vais ouvrir une porte");
            }else if (Checker && other.GetComponent<InteractionObject>() != null && !other.GetComponent<InteractionObject>().Active && !notAvailable) {
                lastObj = other.gameObject;
                notAvailable = true;
                GoToObj = lastObj.transform.position;
                iGoAct = true;
                Checker = false;
                Debug.Log("Je vais ouvrir un switch");
            }
        }
    }

    private void DistanceTrigger () {
        DistancePlayerFather = Vector2.Distance(transform.position,GameController.Instance.PlayerScript.transform.position);
        if(DistancePlayerFather <= DistanceToReact && !notAvailable && !GameController.Instance.ScriptPlayer.GetHidden()){
            SoundManager.Instance.ISpotYou();
            GoChild();
            spdChase = SpeedCap;
            GameController.Instance.FirstCry = true;
            GameController.Instance.SpeedBoost();
        }
        else {
            lastSeen = GameController.Instance.PlayerScript.transform.position;
            GameController.Instance.ReduceBoost();
        }
        
    }
    private void OnTriggerExit2D (Collider2D other) {
        if(other.tag == "Player"){
            GameController.Instance.ReduceBoost();
        }
    }

    public IEnumerator Activate() {
        InteractionObject objScript = lastObj.GetComponent<InteractionObject>();
        Debug.Log("J'ouvre la porte");
        Anim.SetBool("Activate",true);
        yield return new WaitForSeconds(0.5f);
        objScript.objActive();
        lastObj = null;
        Anim.SetBool("Activate",false);
        if (pauseChase) {
            pauseChase = false;
            notAvailable = false;
            iGoTo = true;
        }
        SoundManager.Instance.DoorOpen();
    }
}
