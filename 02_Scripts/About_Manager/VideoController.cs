using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// Update: //@ 2023.11.21 
// Update: //@ 2023.11.23 
//# NOTE: 비디오 제어를 위한 스크립트


//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class VideoController : MonoBehaviour
{
    public static VideoController instance;

    VideoPlayer _videoPlayer;

    //~ -------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;

        _videoPlayer = GetComponent<VideoPlayer>();
    }

    //~ -------------------------------------------------------------------------------
    private void Start()
    {
        _videoPlayer.Prepare();
    }
public void PlayTape()
{
    _videoPlayer.Play();
}
    public void RestartTape()
    {
        _videoPlayer.time = 0;
        //_videoPlayer.Stop();
    }
}
