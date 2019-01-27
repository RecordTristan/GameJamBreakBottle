using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{
    public bool Activate = false;
        public GameObject highlight;
        public GameObject cloneLight; 
    
         // 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void highLight() {

    }

    public virtual void noHighLights() {

    }
    

    public virtual void ActiveHighLight() {

    }
    public virtual void DisactiveHighLight() {

    }
}
