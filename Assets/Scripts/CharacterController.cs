using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public InteractionPlayer ScriptInteraction;
    
    public Rigidbody2D rbChara;

    public float PerteLum;
    public float thrust;
    public float thrustBase;
    public float thrustMax;

    private bool Sens = false;

    

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.PlayerScript = this;
       thrustBase = thrust;
       rbChara = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(ScriptInteraction.CanMove());
        if (Input.GetKeyDown(KeyCode.Space) && ScriptInteraction.CanMove())
        {
            StartCoroutine(ScriptInteraction.Scream());
        }else if(Input.GetKeyDown(KeyCode.E)  && ScriptInteraction.GetScream() && GameController.Instance.ZoneTrigger){
            ScriptInteraction.HideMe();
        }else if(ScriptInteraction.CanMove()){
            if((Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))){
                if ((Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D)) && ScriptInteraction.CanMove())
                {
                    if(Sens){
                        rbChara.velocity = Vector2.zero;
                    }
                    if(rbChara.velocity.magnitude<1){
                        rbChara.AddForce(Vector2.right*thrust*Time.deltaTime);
                        Sens = false;
                    }
                } 
                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q)) && ScriptInteraction.CanMove())
                {
                    if(!Sens){
                        rbChara.velocity = Vector2.zero;
                    }
                    if(rbChara.velocity.magnitude<1){
                        rbChara.AddForce(-Vector2.right*thrust*Time.deltaTime);
                        Sens = true;
                    }
                }
            }else{
                rbChara.velocity = Vector2.zero;
            }
            
        }else{
            rbChara.velocity = Vector2.zero;
        }
        
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<StepfatherScript>() != null)
        {
            //C'est lui
            ScriptInteraction.ActivateReduce();
        }else if(other.GetComponent<InteractionObject>() != null){
            CameraManager.Instance.MakeSpecialCam(Sens);
        }

        if (other.GetComponent<InteractionObject>() || other.GetComponent<HideArea>()) {
            if (other.GetComponent<InteractionObject>() != null)
                other.GetComponent<InteractionObject>().highLight();
            else if (other.GetComponent<HideArea>() != null)
                other.GetComponent<HideArea>().highLight();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<StepfatherScript>() != null)
        {
            //C'est lui
            ScriptInteraction.DisactiveReduce();
        }else if(other.GetComponent<InteractionObject>() != null){
            CameraManager.Instance.StopSpecial();
        }

        if (other.GetComponent<InteractionObject>() || other.GetComponent<HideArea>()) {
            if (other.GetComponent<InteractionObject>() != null)
                other.GetComponent<InteractionObject>().noHighLights();
            else if (other.GetComponent<HideArea>() != null)
                other.GetComponent<HideArea>().noHighLights();
        }
    }

    public void BoostYourSpeed(){
        thrust = thrustMax;
    }
    public void ReduceYourSpeed(){
        thrust = thrustBase;
    }

}
