using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public InteractionPlayer ScriptInteraction;
    
    public Rigidbody2D rbChara;

    public float MaxLife = 1000;
    public float CurrentLife = 1000;

    public float PerteLum;
    public float thrust;
    public float thrustBase;
    public float thrustMax;

    private Animator Anim;
    private bool Sens = false;

    public GameObject ZoneTr;
    private bool Jump = false;
    private bool MaxJump = false;

    public AudioSource MySound;
    public List<AudioClip> Walk;
    int tracklistSFX = 0;
    bool CwalkOnPlay = false;
    // Start is called before the first frame update
    void Start()
    {
        Anim = this.GetComponent<Animator>();
        GameController.Instance.PlayerScript = this;
       thrustBase = thrust;
       rbChara = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Jump){
            JumpFunc();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && ScriptInteraction.CanMove())
        {
            StartCoroutine(ScriptInteraction.Scream());
        }
        if(Input.GetKeyDown(KeyCode.E)  && ScriptInteraction.GetScream()){
            if(ZoneTr != null){
                if(ZoneTr.GetComponent<HideArea>() != null || GameController.Instance.ScriptPlayer.GetHidden()){
                ScriptInteraction.HideMe();
                }else{
                    Debug.Log(ZoneTr.GetComponent<InteractionObject>().TagOfObject);
                    if(ZoneTr.GetComponent<InteractionObject>().TagOfObject == "Door"){
                        SoundManager.Instance.DoorLook();
                        //Jump = true;
                    }else{
                        //Jump = true;
                    }
                }
            }else if(GameController.Instance.ScriptPlayer.GetHidden()){
                ScriptInteraction.HideMe();
            }
            
            
        }
    }
    void FixedUpdate()
    {
        if(Jump)
            return;
        if(ScriptInteraction.CanMove()){
            if((Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))){
                if(!CwalkOnPlay){
                    CwalkOnPlay = true;
                    StartCoroutine(CFootSteps());
                }
                if ((Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D)) && ScriptInteraction.CanMove())
                {
                    if(Sens){
                        rbChara.velocity = Vector2.zero;
                        Anim.SetBool("Walk",false);
                    }
                    if(rbChara.velocity.magnitude<1){
                        rbChara.AddForce(Vector2.right*thrust*Time.deltaTime);
                        Sens = false;
                        transform.localScale = new Vector3(1,1,1);
                        SoundManager.Instance.childWalk = true;
                        Anim.SetBool("Walk",true);
                    }
                } 
                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q)) && ScriptInteraction.CanMove())
                {
                    if(!Sens){
                        rbChara.velocity = Vector2.zero;
                        Anim.SetBool("Walk",false);
                    }
                    if(rbChara.velocity.magnitude<1){
                        rbChara.AddForce(-Vector2.right*thrust*Time.deltaTime);
                        transform.localScale = new Vector3(-1,1,1);
                        Sens = true;
                        SoundManager.Instance.childWalk = true;
                        Anim.SetBool("Walk",true);
                    }
                }
            }else{
                rbChara.velocity = Vector2.zero;
                SoundManager.Instance.childWalk = false;
                Anim.SetBool("Walk",false);
            }
            
        }else{
            rbChara.velocity = Vector2.zero;
            Anim.SetBool("Walk",false);
        }
        
        
    }

    IEnumerator CFootSteps() {
        MySound.PlayOneShot(Walk[tracklistSFX]);
        yield return new WaitForSeconds(0.7f);
        tracklistSFX ++;
        if (tracklistSFX >= 2) {
            tracklistSFX = 0;
        }
        CwalkOnPlay = false;
    }

    private float step =0;
    public void JumpFunc(){
        if(!MaxJump){
            if(step <=0.2f){
                step = 5 * Time.deltaTime;
                transform.localPosition = Vector2.MoveTowards(transform.localPosition,new Vector2(transform.localPosition.x,transform.localPosition.y+0.2f) , step);
            }else{
                MaxJump = true;
                step =0;
            }
        }else{
            if(step <=0.2f){
                step = 5 * Time.deltaTime;
                transform.localPosition = Vector2.MoveTowards(transform.localPosition,new Vector2(transform.localPosition.x,transform.localPosition.y-0.2f) , step);
            }else{
                MaxJump = false;
                step =0;
                Jump = false;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<StepfatherScript>() != null)
        {
            //C'est lui
            ScriptInteraction.ActivateReduce();
        }

        if (other.GetComponent<SceneObject>()!= null) {
            ZoneTr = other.gameObject;
            other.GetComponent<SceneObject>().ActiveHighLight();
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
        if (other.GetComponent<SceneObject>()!= null) {
            other.GetComponent<SceneObject>().DisactiveHighLight();
        }
        if(ZoneTr != null){
            if(ZoneTr == other.gameObject){
                ZoneTr = null;
            }
        }
    }

    public void BoostYourSpeed(){
        thrust = thrustMax;
    }
    public void ReduceYourSpeed(){
        thrust = thrustBase;
    }

}
