using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideArea : SceneObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        cloneLight = Instantiate(highlight, transform.position, transform.rotation);
    }

    public override void noHighLights() {
        Destroy(cloneLight);
    }
}
