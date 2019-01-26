using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadObject : MonoBehaviour
{
    public float PointReduce = 0.1f;
    private bool Activate = false;

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
        
        GameController.Instance.ScriptPlayer.ReduceCircle(PointReduce);
    }


    public void OnTriggerEnter2D(Collider2D other){
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
