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

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.StepFather = this;
        MinspdChase = spdChase;
        GameObject clone = Instantiate(pointRound, transform.position, transform.rotation);
        rondoPoints.Add(clone.transform.position);
        GameObject clone1 = Instantiate(pointRound, new Vector2(transform.position.x + 10f, transform.position.y), transform.rotation);
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
                rondoPoints.RemoveAt(1);
                rondoPoints.RemoveAt(0);
                if (lookinAt) {
                    Debug.Log("Je partir à droate");
                    GameObject clone2 = Instantiate (pointRound, new Vector2(GoTo.x +10f, GoTo.y), transform.rotation);
                    GameObject clone3 = Instantiate (pointRound, new Vector2(GoTo.x -10f, GoTo.y), transform.rotation);
                    rondoPoints.Add(clone2.transform.position);
                    rondoPoints.Add(clone3.transform.position);
                    roundPlace = 0;
                }
                else {
                    Debug.Log("Je partir à gôche");
                    GameObject clone2 = Instantiate (pointRound, new Vector2(GoTo.x +10f, GoTo.y), transform.rotation);
                    GameObject clone3 = Instantiate (pointRound, new Vector2(GoTo.x -10f, GoTo.y), transform.rotation);
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
        Debug.Log("Je vais voir mon fils");
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
            if (other.GetComponent<InteractionObject>() != null && iGoTo && !other.GetComponent<InteractionObject>().Active) {
                iGoTo = false;
                lastObj = other.gameObject;
                GoToObj = other.transform.position;
                iGoAct = true;
                pauseChase = true;
                Debug.Log("Je vais ouvrir une porte");
            }else if (Checker && other.GetComponent<InteractionObject>() != null && !other.GetComponent<InteractionObject>().Active) {
                lastObj = other.gameObject;
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
        if(DistancePlayerFather <= DistanceToReact){
            GoChild();
            GameController.Instance.SpeedBoost();
        }
        else {
            lastSeen = GameController.Instance.PlayerScript.transform.position;
            GameController.Instance.ReduceBoost();
           /* rondoPoints.RemoveAt(1);
            rondoPoints.RemoveAt(0);
            if (lookinAt) {
                Debug.Log("droate");
                GameObject clone4 = Instantiate (pointRound, new Vector2(lastSeen.x +10, lastSeen.y), transform.rotation);
                GameObject clone5 = Instantiate (pointRound, new Vector2(lastSeen.x - 10, lastSeen.y), transform.rotation);
                rondoPoints.Add(clone4.transform.position);
                rondoPoints.Add(clone5.transform.position);
                onRound = true;
            }
            else {
                Debug.Log("gôche");
                GameObject clone4 = Instantiate (pointRound, new Vector2(lastSeen.x - 10, lastSeen.y), transform.rotation);
                GameObject clone5 = Instantiate (pointRound, new Vector2(lastSeen.x + 10, lastSeen.y), transform.rotation);
                rondoPoints.Add(clone4.transform.position);
                rondoPoints.Add(clone5.transform.position);
                onRound = true;
            }*/
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
            iGoTo = true;
        }
    }
}
