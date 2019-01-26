using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public Image ViewBarLight;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        ViewBarLight.fillAmount = (GameController.Instance.ScriptPlayer.GetLightRange().magnitude)/(GameController.Instance.ScriptPlayer.MaxLightRange.magnitude);
    }
}
