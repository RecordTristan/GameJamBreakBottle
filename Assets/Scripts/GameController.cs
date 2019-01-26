﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public GameObject Essai;
    public bool ZoneTrigger = false;

    public CharacterController PlayerScript;
    public InteractionPlayer ScriptPlayer;
    public StepfatherScript StepFather;

    public float MaxJauge = 10;
    public float AddJauge = 2;
    public float ReduceInTime = 0.02f;
    private float JaugeAlert = 0;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        ReduceAlert();
    }

    public bool CanHide(){
        if(ZoneTrigger && ScriptPlayer.CanMove()){
            return true;
        }else{
            return ZoneTrigger;
        }
    }

    public void AugmentAlert(){
        JaugeAlert += AddJauge;
        if(MaxJauge < JaugeAlert){
            JaugeAlert = MaxJauge;
        }
        MajAlert();
    }

    public void ReduceAlert(){
        if(JaugeAlert>0){
            JaugeAlert -= ReduceInTime * Time.deltaTime;
        }else{
            JaugeAlert = 0;
        }
        
        MajAlert();
    }

    public void MajAlert(){
        StepFather.spdChase = StepFather.MinspdChase + JaugeAlert;
    }

    public void SpeedBoost(){
        PlayerScript.BoostYourSpeed();
    }
    public void ReduceBoost(){
        PlayerScript.ReduceYourSpeed();
    }
}