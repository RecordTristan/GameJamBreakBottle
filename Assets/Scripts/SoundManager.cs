using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
 
    public AudioSource CreapySound;
    public AudioSource MusicGame;
    public List<AudioClip> soundEffect = new List<AudioClip>();
    public bool childWalk = false;
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
        if (childWalk) {

        }
    }

    void UpdateCreapySound(){
        //CreapySound.volume = Mathf.Abs((GameController.Instance.ScriptPlayer.GetLightRange()-2.5f)/(GameController.Instance.ScriptPlayer.MaxLightRange-2.5f)-1);
        //MusicGame.volume = Mathf.Abs((GameController.Instance.ScriptPlayer.GetLightRange()-2.5f)/(GameController.Instance.ScriptPlayer.MaxLightRange-2.5f)+0.3f);
    }
}
