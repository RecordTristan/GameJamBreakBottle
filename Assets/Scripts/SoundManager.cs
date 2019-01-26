using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
 
    public AudioSource CreapySound;
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
        UpdateCreapySound();
    }

    void UpdateCreapySound(){
        CreapySound.volume = Mathf.Abs(GameController.Instance.ScriptPlayer.GetLightRange()/4-1);
    }
}
