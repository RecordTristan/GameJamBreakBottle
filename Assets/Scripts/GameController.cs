using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public GameObject Essai;
    public bool ZoneTrigger = false;

    public CharacterController PlayerScript;
    public InteractionPlayer ScriptPlayer;
    public List<GameObject> decorDrop = new List<GameObject>();
    public StepfatherScript StepFather;

    public Image fadeIn;
    private bool fadeInActive = false;

    public float MaxJauge = 10;
    public float AddJauge = 2;
    public float ReduceInTime = 0.02f;
    private float JaugeAlert = 0;

    public bool FirstCry = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ZoneTrigger = false;

    }

    // Update is called once per frame
    void Update()
    {
        ReduceAlert();
        if (fadeInActive) {
            Color tempColor = fadeIn.color;
            if (tempColor.a < 1) {
                tempColor.a = tempColor.a + Time.deltaTime;
                fadeIn.color = tempColor;
            }
            else if (tempColor.a >= 1) {
                fadeInActive = false;
                StartCoroutine(Restart());
            }
            
        }
    }

    public bool CanHide(){
        if(ZoneTrigger && ScriptPlayer.CanMove()){
            return true;
        }else{
            return ZoneTrigger;
        }
    }

    public void AugmentAlert(){
        JaugeAlert += AddJauge;
        if(MaxJauge < JaugeAlert){
            JaugeAlert = MaxJauge;
        }
        MajAlert();
    }

    public void ReduceAlert(){
        if(JaugeAlert>0){
            JaugeAlert -= ReduceInTime * Time.deltaTime;
        }else{
            JaugeAlert = 0;
        }
        
        MajAlert();
    }

    public void MajAlert(){
        StepFather.spdChase = StepFather.MinspdChase + JaugeAlert;
    }

    public void SpeedBoost(){
        PlayerScript.BoostYourSpeed();
    }
    public void ReduceBoost(){
        PlayerScript.ReduceYourSpeed();
    }

    public void GameOver() {
        // Player.GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerScript.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerScript.enabled = false;
        SoundManager.Instance.GOSound();
        fadeInActive = true; 
    }

    public IEnumerator Restart() {
        Scene scene = SceneManager.GetActiveScene();
        yield return new WaitForSeconds(2.1f);
        SceneManager.LoadScene(scene.name);
    }
    // (GameController.Instance.ScriptPlayer.GetLightRange().magnitude)/(GameController.Instance.ScriptPlayer.MaxLightRange.magnitude);
}
