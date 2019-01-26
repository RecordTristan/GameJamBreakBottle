using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPlayer : MonoBehaviour
{
    public float TimeScream;
    private bool ScreamFinish = true;

    public float ReducterOfTime = 0.0001f;
    public Vector3 MaxLightRange;
    public float MaxRangeCollider;

    public float ReduceSizeStepFather = 0.005f;

    public GameObject LocPos;

    private SpriteRenderer MyAnxiety;

    private CircleCollider2D ColliderPlayer;
    private bool ReduceCollider;
    private bool IsOnSafeArea = false;

    public float AugmentAfterScream = 1f;

    private bool Hiden = false;

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.ScriptPlayer = this;
        ColliderPlayer = this.GetComponent<CircleCollider2D>();
        MyAnxiety = this.transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
        MaxLightRange = MyAnxiety.transform.localScale;
        MaxRangeCollider = ColliderPlayer.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (ReduceCollider)
        {
            ReduceCircle(ReduceSizeStepFather);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ColliderPlayer.radius = MaxRangeCollider;
            MyAnxiety.transform.localScale = MaxLightRange;
        }
        if (!IsOnSafeArea)
        {
            ReduceCircle(ReducterOfTime);
        }
        MyAnxiety.transform.position = this.transform.position;
    }

    public IEnumerator Scream()
    {
        ScreamFinish = false;
        CameraManager.Instance.ShakeTime(TimeScream);
        yield return new WaitForSeconds(TimeScream);
        GameController.Instance.AugmentAlert();
        CreatePos();
        GameController.Instance.ScriptPlayer.AugmentCircle(AugmentAfterScream);
        GameController.Instance.StepFather.onRound = false;
        GameController.Instance.StepFather.GoChild();
        ScreamFinish = true;
    }

    public void CreatePos(){
        GameObject gm = Instantiate(LocPos);
        gm.transform.position = this.transform.position;
        GameController.Instance.StepFather.gm = gm;
    }

    public bool GetScream(){
        return ScreamFinish;
    }
    public bool GetHidden(){
        return Hiden;
    }

    public bool CanMove()
    {
        return ScreamFinish && !Hiden;
    }

    public void HideMe(){
        if(Hiden){
            Hiden = false;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = true;
        }else{
            Hiden = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    public void ActivateReduce()
    {
        ReduceCollider = true;
    }
    public void DisactiveReduce()
    {
        ReduceCollider = false;
    }

    public void ReduceCircle(float Reducter)
    {
        if(MyAnxiety.transform.localScale.x >=3.7f){
            ColliderPlayer.radius -= Reducter;
            MyAnxiety.transform.localScale = new Vector3(MyAnxiety.transform.localScale.x - Reducter * 20,MyAnxiety.transform.localScale.y - Reducter * 20,MyAnxiety.transform.localScale.z - Reducter * 20);
        }
    }
    public void AugmentCircle(float Augmenter)
    {
    }

    public Vector3 GetLightRange(){
        return MyAnxiety.transform.localScale;
    }

}
