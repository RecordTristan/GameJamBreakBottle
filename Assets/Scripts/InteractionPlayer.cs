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
    public float alphaDescending = 0.05f;

    public Color ColorOfObject = new Color (1f,1f,1f,0f);
    public float Speed=6;
    private bool ActivateScream = false;
    private float Adder = 0;



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
        if (!IsOnSafeArea)
        {
            ReduceCircle(ReducterOfTime);
        }
        MyAnxiety.transform.position = this.transform.position;
        if(ActivateScream && Adder<AugmentAfterScream){
            Adder += AugmentAfterScream/Speed;
            GameController.Instance.ScriptPlayer.AugmentCircle(Adder);
        }else{
            ActivateScream = false;
            Adder=0;
        }
    }

    public IEnumerator Scream()
    {
        Debug.Log("OK");
        ScreamFinish = false;
        SoundManager.Instance.ScreamSong();
        CameraManager.Instance.ShakeTime(TimeScream);
        yield return new WaitForSeconds(TimeScream);
        ActivateScream = true;
        GameController.Instance.FirstCry = true;
        GameController.Instance.AugmentAlert();
        CreatePos();
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
        else {
            GameController.Instance.GameOver();
        }
        float Calcul = ColorOfObject.a + Reducter;
        if(Calcul>1){
            Calcul = 1;
        }else if(Calcul <=0){
            Calcul = 0;
        }
        ColorOfObject = new Color(1f, 1f, 1f, ColorOfObject.a + Reducter);
        foreach(GameObject items in GameController.Instance.decorDrop) {
            Debug.Log(items.name);
            items.GetComponent<SpriteRenderer>().color = ColorOfObject;
        }
        
    }
    public void AugmentCircle(float Augmenter)
    {
        if(MyAnxiety.transform.localScale.x <= MaxLightRange.x){
            ColliderPlayer.radius += Augmenter;
            MyAnxiety.transform.localScale = new Vector3(MyAnxiety.transform.localScale.x + Augmenter * 20,MyAnxiety.transform.localScale.y + Augmenter * 20,MyAnxiety.transform.localScale.z + Augmenter * 20);
        }
        if(MyAnxiety.transform.localScale.x > MaxLightRange.x){
            ColliderPlayer.radius = MaxRangeCollider;
            MyAnxiety.transform.localScale = MaxLightRange;
        }
        float Calcul = ColorOfObject.a - Augmenter;
        if(Calcul<0){
            Calcul = 0;
        }
        ColorOfObject = new Color(1f, 1f, 1f, ColorOfObject.a - Augmenter);
        foreach(GameObject items in GameController.Instance.decorDrop) {
            items.GetComponent<SpriteRenderer>().color = ColorOfObject;
        }
    }

    public Vector3 GetLightRange(){
        return MyAnxiety.transform.localScale;
    }

}
