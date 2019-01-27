using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : SceneObject
{

    public bool Active;
    public GameObject linkedBadbox;
    // Start is called before the first frame update
    public Light Lightning;
    public float Speed;
    private float TimerLight;
    private bool Sens = false;

    private Animator Anim;
    void Start()
    {
        highLight();
        Lightning = transform.GetChild(0).GetComponent<Light>();
        if(GetComponent<Animator>() != null){
            Anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Activate){
            if(Sens){
                TimerLight += Speed * Time.deltaTime;
                Lightning.intensity = Mathf.Lerp(Lightning.intensity,6,TimerLight);
                if(Lightning.intensity <=6){
                    Sens = false;
                    TimerLight = 0;
                }
            }else{
                TimerLight += Speed * Time.deltaTime;
                Lightning.intensity = Mathf.Lerp(Lightning.intensity,9,TimerLight);
                if(Lightning.intensity >=9){
                    Sens = true;
                    TimerLight = 0;
                }
            }
            
            
        }
    }

    public void objActive() {
        Active = true;
        if (this.tag == "Door") {
            Debug.Log("Im unlocked");
            if(GetComponent<Animator>() != null){
                Anim.SetBool("Open",true);
            }
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (this.tag == "Switch") {
            Debug.Log("je désactive cette zone de FDP");
            linkedBadbox.SetActive(false);
        }
    }

    public override void highLight() {
        cloneLight = Instantiate(highlight);
    }

    public override void noHighLights() {
        Destroy(cloneLight);
    }

    public override void ActiveHighLight() {
        Activate = true;
    }
    public override void DisactiveHighLight() {
        Activate = false;
    }
}
