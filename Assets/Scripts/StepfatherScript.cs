using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepfatherScript : MonoBehaviour
{
    public GameObject childSpot;
    public GameObject lastObj;

    public float MinspdChase = 5;
    public float spdChase = 5;

    private float DistancePlayerFather;
    public float DistanceToReact=3;
    public Vector2 lastSeen;

    private Vector3 GoTo;
    private Vector3 GoToObj;

    public GameObject gm;
    public bool iGoTo = false;
    public bool iGoAct = false;
    public bool Checker = false;
    private bool pauseChase = false;

    List<Vector2> rondoPoints = new List<Vector2>(); 
    public GameObject pointRound;
    public int nbrRoundMax = 6;
    public int roundPlace = 0;
    public bool onRound = true;

    public bool lookinAt = true;
    public bool notAvailable = false;

    public GameObject clone;
    public GameObject clone1;
    public GameObject clone2;
    public GameObject clone3;

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.StepFather = this;
        MinspdChase = spdChase;
        clone = Instantiate(pointRound, transform.position, transform.rotation);
        rondoPoints.Add(clone.transform.position);
        clone1 = Instantiate(pointRound, new Vector2(transform.position.x + 10f, transform.position.y), transform.rotation);
        rondoPoints.Add(clone1.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (iGoTo) {
            onRound = false;
            if (this.transform.position != GoTo) {
                float step = spdChase * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, GoTo, step);
                if(gm != null){
                    Destroy(gm);
                }
            }
            else {
                Debug.Log("Gé pa vu mon fisse");
                Checker = true;
                Destroy(clone);
                Destroy(clone1);
                Destroy(clone2);
                Destroy(clone3);
                rondoPoints.RemoveAt(1);
                rondoPoints.RemoveAt(0);

                if (lookinAt) {
                    Debug.Log("Je partir à droate");
                    clone2 = Instantiate (pointRound, new Vector2(GoTo.x +10f, GoTo.y), transform.rotation);
                    clone3 = Instantiate (pointRound, new Vector2(GoTo.x -10f, GoTo.y), transform.rotation);
                    rondoPoints.Add(clone2.transform.position);
                    rondoPoints.Add(clone3.transform.position);
                    roundPlace = 0;
                }
                else {
                    Debug.Log("Je partir à gôche");
                    clone2 = Instantiate (pointRound, new Vector2(GoTo.x +10f, GoTo.y), transform.rotation);
                    clone3 = Instantiate (pointRound, new Vector2(GoTo.x -10f, GoTo.y), transform.rotation);
                    rondoPoints.Add(clone3.transform.position);
                    rondoPoints.Add(clone2.transform.position);
                    roundPlace = 0;
                }
                iGoTo = false;
                onRound = true;
            }
        }
        if (iGoAct) {
            if (this.transform.position != GoToObj) {
                Debug.Log("Je vais activer la porte");
                float step = spdChase * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, GoToObj, step);
            }
            else {
                iGoAct = false;
                StartCoroutine(Activate());
            }
        }
        if (Checker) {
            this.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        }
        else if (!Checker) {
            this.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        }
        DistanceTrigger ();

        // Liste des rondes ici:
        if (rondoPoints.Count > nbrRoundMax) {
            Destroy(clone1);
            Destroy(clone2);
            Destroy(clone2);
            Destroy(clone3);
            rondoPoints.RemoveAt(1);
            rondoPoints.RemoveAt(0);
        }

        if (onRound) {
            if (this.transform.position.x != rondoPoints[roundPlace].x) {
                float step = spdChase * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, rondoPoints[roundPlace], step);
            }
            else {
                roundPlace ++;
                if (roundPlace == rondoPoints.Count) {
                    Debug.Log("Je m'avance en arrière");
                    roundPlace = 0;
                }
            }
        }
    }

    public void GoChild()
    {
        onRound = false;
        GoTo = GameController.Instance.ScriptPlayer.transform.position;
        if (this.transform.position.x < GoTo.x)
            lookinAt = true;
        else
            lookinAt = false;
        iGoTo = true;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if(other.gameObject == gm){
            Debug.Log("Hello");
            iGoTo = true;
        }else{
            if (other.GetComponent<InteractionObject>() != null && iGoTo && !other.GetComponent<InteractionObject>().Active && !notAvailable) {
                iGoTo = false;
                notAvailable = true;
                lastObj = other.gameObject;
                GoToObj = other.transform.position;
                iGoAct = true;
                pauseChase = true;
                Debug.Log("Je vais ouvrir une porte");
            }else if (Checker && other.GetComponent<InteractionObject>() != null && !other.GetComponent<InteractionObject>().Active && !notAvailable) {
                lastObj = other.gameObject;
                notAvailable = false;
                GoToObj = lastObj.transform.position;
                iGoAct = true;
                Checker = false;
            }
        }
        //
        if(other.tag == "Player"){
            Debug.Log("Hello");
        }
    }

    private void DistanceTrigger () {
        DistancePlayerFather = Vector2.Distance(transform.position,GameController.Instance.PlayerScript.transform.position);
        if(DistancePlayerFather <= DistanceToReact && !notAvailable){
            GoChild();
            GameController.Instance.SpeedBoost();
        }
        else {
            lastSeen = GameController.Instance.PlayerScript.transform.position;
            GameController.Instance.ReduceBoost();
        }
        
    }
    private void OnTriggerExit2D (Collider2D other) {
        if(other.tag == "Player"){
            GameController.Instance.ReduceBoost();
        }
    }

    public IEnumerator Activate() {
        InteractionObject objScript = lastObj.GetComponent<InteractionObject>();
        Debug.Log("J'ouvre la porte");
        yield return new WaitForSeconds(1f);
        objScript.objActive();
        lastObj = null;
        if (pauseChase) {
            pauseChase = false;
            notAvailable = false;
            iGoTo = true;
        }
    }
}
