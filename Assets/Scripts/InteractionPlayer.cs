using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPlayer : MonoBehaviour
{
    public float TimeScream;
    private bool ScreamFinish = true;

    public float ReducterOfTime = 0.0001f;
    public float MaxLightRange;
    public float MaxRangeCollider;

    public float ReduceSizeStepFather = 0.005f;

    public GameObject LocPos;

    private Light MyAnxiety;

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
        MyAnxiety = this.transform.GetChild(0).GetComponent<Light>();
        MaxLightRange = MyAnxiety.range;
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
            MyAnxiety.range = MaxLightRange;
        }
        if (!IsOnSafeArea)
        {
            ReduceCircle(ReducterOfTime);
        }
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
        if(ScreamFinish && !Hiden){
            return true;
        }else{
            return false;
        }
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
        ColliderPlayer.radius -= Reducter;
        MyAnxiety.range -= Reducter*20;
    }
    public void AugmentCircle(float Augmenter)
    {
        if(MaxLightRange > MyAnxiety.range){
            if(MaxLightRange < MyAnxiety.range + Augmenter*20){
                ColliderPlayer.radius = MaxRangeCollider;
                MyAnxiety.range = MaxLightRange;
            }else{
                ColliderPlayer.radius += Augmenter;
                MyAnxiety.range += Augmenter*20;
            }
        }
    }

    public float GetLightRange(){
        return MyAnxiety.range;
    }

}
