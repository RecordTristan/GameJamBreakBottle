using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : SceneObject
{

    public bool Active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void objActive() {
        Active = true;
    }

    public override void highLight() {
        cloneLight = Instantiate(highlight, transform.position, transform.rotation);
    }

    public override void noHighLights() {
        Destroy(cloneLight);
    }
}
