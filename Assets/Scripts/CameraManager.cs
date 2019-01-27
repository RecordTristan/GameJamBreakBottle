using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Vector3 originalPos;
    public static CameraManager Instance;
    public float Magnitude =0.15f;
    float MagnPlus;

    bool Shaker;
    public float TimerFinish;
    float TimerShaker;

    float Height;

    bool SpecialCam = false;

    Vector3 FuturPos;
    public float SpeedMove = 4;
    float TimerMoveCam=0;
    bool ActiveSpe = false;
    // Use this for initialization
    void Start () {
        Height = transform.localPosition.y;
        originalPos = this.transform.localPosition;
	}

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
    // Update is called once per frame
    void Update () {
        if (Shaker)
        {
            if (TimerShaker > TimerFinish)
            {
                Shaker = false;
                TimerShaker = 0;
                MagnPlus = 0;
                this.transform.position = originalPos;
            }
            else
            {
                TimerShaker += Time.deltaTime;
                Shake(MagnPlus);
            }
        }else{
            TimerMoveCam = 0.02f;
            transform.position = Vector3.Lerp(transform.position, new Vector3(GameController.Instance.ScriptPlayer.transform.position.x,GameController.Instance.ScriptPlayer.transform.position.y+Height,-10),TimerMoveCam);          
        }
	}

    public void Shake (float magnitude)
    {
        float x = Random.Range(-1f, 1f) * magnitude;
        float y = Random.Range(-1f, 1f) * magnitude;

        transform.position = new Vector3(originalPos.x+x, originalPos.y+ y, originalPos.z);

    }

    public void ShakeTime(float myTime)
    {
        originalPos = Camera.main.transform.position;
        TimerFinish = myTime;
        MagnPlus = Magnitude;
        Shaker = true;
    }

    public void ReturnPos()
    {
        transform.position = originalPos;
    }

    public void MakeSpecialCam(bool sens){
        ActiveSpe = true;
        SpecialCam = true;
        TimerMoveCam = 0;
        originalPos = transform.position;
        if(sens){
            FuturPos = new Vector3(originalPos.x-20,originalPos.y,originalPos.z);
        }else{
            FuturPos = new Vector3(originalPos.x+20,originalPos.y,originalPos.z);
        }
    }
    public void StopSpecial(){
        TimerMoveCam = 0;
        SpecialCam = false;
    }

}
