using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodObject : MonoBehaviour
{
    public float Benefit = 0.1f;
    private bool Activate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActivateArea();
    }


    public void ActivateArea(){
        if(!Activate)
            return;
        
        GameController.Instance.ScriptPlayer.AugmentCircle(Benefit);
    }


    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player" && !other.isTrigger){
            Activate = true;
            SoundManager.Instance.SafeSound();
            Debug.Log("OK");
        }
    }
    public void OnTriggerStay2D(Collider2D other){
        if(other.tag == "Player" && !other.isTrigger){
            Activate = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player" && !other.isTrigger){
            Activate = false;
        }
    }
}
