using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideArea : SceneObject
{
    // Start is called before the first frame update
    public Light Lightning;
    public float Speed;
    private float TimerLight;
    private bool Sens = false;
    void Start()
    {
        highLight();
        Lightning = transform.GetChild(0).GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Activate){
            if(Sens){
                TimerLight += Speed * Time.deltaTime;
                Lightning.intensity = Mathf.Lerp(9,6,TimerLight);
                if(Lightning.intensity <=6){
                    Sens = false;
                    TimerLight = 0;
                }
            }else{
                TimerLight += Speed * Time.deltaTime;
                Lightning.intensity = Mathf.Lerp(6,9,TimerLight);
                if(Lightning.intensity >=9){
                    Sens = true;
                    TimerLight = 0;
                }
            }
            
            
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameController.Instance.ZoneTrigger = true;
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if(!GameController.Instance.ScriptPlayer.GetHidden()){
            GameController.Instance.ZoneTrigger = false;
        }
    }

    public override void highLight() {
        cloneLight = Instantiate(highlight);
        cloneLight.transform.parent = this.transform;
        cloneLight.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,-2f);
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
