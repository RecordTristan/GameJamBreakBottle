using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideArea : SceneObject
{
    // Start is called before the first frame update
    public Light Lightning;
    public float Speed;
    private float TimerLight;
    private bool Sens = true;
    private bool onLights = false;
    private bool noLights = true;
    private SpriteRenderer sprAlpha;

    private Animator anim;
    void Start()
    {
        highLight();
        if (this.transform.parent != null){
            sprAlpha = this.transform.parent.GetComponent<SpriteRenderer>();
        }
        if(this.GetComponent<Animator>() != null){
            anim = this.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Activate){
           if (onLights && !noLights) {
                if(Sens){
                    if (TimerLight < 0.1f) {
                        Debug.Log(TimerLight);
                        TimerLight += Speed * Time.deltaTime;
                        float alphaSprite = sprAlpha.color.a;
                        alphaSprite = Mathf.Lerp(alphaSprite,120f/255f,TimerLight);
                        sprAlpha.color = new Color(sprAlpha.color.r,sprAlpha.color.g,sprAlpha.color.b,alphaSprite);
                    }else{
                        TimerLight=0;
                        Sens = false;
                        sprAlpha.color = new Color(sprAlpha.color.r,sprAlpha.color.g,sprAlpha.color.b,120f/255f);
                    }
                }else{
                    if (TimerLight < 0.1f) {
                        TimerLight += Speed * Time.deltaTime;
                        float alphaSprite = sprAlpha.color.a;
                        alphaSprite = Mathf.Lerp(alphaSprite,60f/255f,TimerLight);
                        sprAlpha.color = new Color(sprAlpha.color.r,sprAlpha.color.g,sprAlpha.color.b,alphaSprite);
                    }else{
                        TimerLight = 0;
                        Sens = true;
                        sprAlpha.color = new Color(sprAlpha.color.r,sprAlpha.color.g,sprAlpha.color.b,60f/255f);
                    }
                }
                
                
            }
        }
        if(GameController.Instance.ScriptPlayer.GetHidden()){
            if(anim != null){
                anim.SetBool("IsHiding",true);
            }
        }else{
            if(anim != null){
                anim.SetBool("IsHiding",false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            GameController.Instance.ZoneTrigger = true;
        }
        
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" && !GameController.Instance.ScriptPlayer.GetHidden()){
            GameController.Instance.ZoneTrigger = false;
        }
        if(other.tag=="Player"){
            sprAlpha.color = new Color(sprAlpha.color.r,sprAlpha.color.g,sprAlpha.color.b,0f);
        }
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
