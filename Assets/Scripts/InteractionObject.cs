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
    private bool Sens = true;

    private Animator Anim;
    private bool onLights = false;
    private bool noLights = false;
    private SpriteRenderer sprAlpha;

    public string TagOfObject;
    public Sprite NewObject;

    void Start()
    {
        //highLight();
        // Lightning = transform.GetChild(0).GetComponent<Light>();
        if(GetComponent<Animator>() != null){
            Anim = GetComponent<Animator>();
        }
        if (this.transform.parent != null)
            sprAlpha = this.transform.parent.GetComponent<SpriteRenderer>();
        else   
            return;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Activate){
           if (onLights && !noLights) {
                if(Sens){
                    if (sprAlpha.color.a <62) {
                        Debug.Log("Je me lumiere haute");
                        TimerLight += Speed * Time.deltaTime;
                        float alphaSprite = sprAlpha.color.a;
                        alphaSprite = Mathf.Lerp(alphaSprite,1f,TimerLight);
                        sprAlpha.color = new Color(sprAlpha.color.r,sprAlpha.color.g,sprAlpha.color.b,alphaSprite);
                    }else{
                        Sens = false;
                    }
                }else{
                    if (sprAlpha.color.a >0) {
                        Debug.Log("Je me lumière basse");
                        TimerLight += Speed * Time.deltaTime;
                        float alphaSprite = sprAlpha.color.a;
                        alphaSprite = Mathf.Lerp(alphaSprite,0f,TimerLight);
                        sprAlpha.color = new Color(sprAlpha.color.r,sprAlpha.color.g,sprAlpha.color.b,alphaSprite);
                    }else{
                        Sens = true;
                    }
                }
                
                
            }
        }

        
        
        
    }

    public void objActive() {
        Active = true;
        if (this.tag == "Door") {
            SoundManager.Instance.DoorOpen();
            if(GetComponent<Animator>() != null){
                Anim.SetBool("Open",true);
            }
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (this.tag == "Switch") {
            linkedBadbox.SetActive(false);
            SoundManager.Instance.LightSwitch();
        }
        this.GetComponent<SpriteRenderer>().sprite = NewObject;
    }

    public override void highLight() {
        // cloneLight = Instantiate(highlight);
        noLights = false;
        onLights = true;
    }

    public override void noHighLights() {
        // Destroy(cloneLight);
        onLights = false;
        noLights = false;
    }

    public override void ActiveHighLight() {
        Activate = true;
    }
    public override void DisactiveHighLight() {
        Activate = false;
    }
}
