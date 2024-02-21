using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayCutscene : MonoBehaviour
{
    public static bool IsPausing = false;
    public GameObject playCutscene;
    double time;
    double currentTime;
    // Use this for initialization
    void Start()
    {

        
    }


    // Update is called once per frame
    void Update()
    {
        if (playVideo)
        {
            currentTime = playCutscene.transform.Find("GameObject").GetComponent<VideoPlayer>().time;
            if (currentTime >= time)
            {
                playCutscene.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerGame")
        {
            CheckTimeLineAndPlay(playCutscene);
            CheckVideoPlayer(playCutscene);
        }

    }
    void CheckTimeLineAndPlay(GameObject Cutscene)
    {
        if(Cutscene.GetComponent<PlayableDirector>() != null)
        {
            Debug.Log("FoundTimeLine");
            Cutscene.SetActive(true);
            while (Cutscene.GetComponent<PlayableDirector>().state == PlayState.Playing)
            {
                IsPausing = true;
            }
            IsPausing = false;
            Cutscene.SetActive(false);
            Destroy(gameObject);
        }
    }
    bool playVideo = false;
    private void CheckVideoPlayer(GameObject Cutscene)
    {
        Debug.Log("FoundVideo");
        if (Cutscene.GetComponent<RawImage>() != null)
        {
            time = Cutscene.transform.Find("GameObject").GetComponent<VideoPlayer>().clip.length;
            Cutscene.SetActive(true) ;
            playVideo = true ;
        }
    }
}
