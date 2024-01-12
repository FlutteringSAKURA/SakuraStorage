using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using MEC;

namespace mARTians
{
    public class CTimelineController : MonoBehaviour
    {
        public List<PlayableDirector> playableDirectors;
        public List<TimelineAsset> timelines;
        public bool timlineActivate = false;

        //* > > > TEST 
        // public PlayableDirector playableDirector;
        //* < < < <

        public void Play()
        {
            //* get set ready...
            // foreach (PlayableDirector playableDirector in playableDirectors)
            // {
            //     playableDirector.Play();
            // }
            // playableDirector.Play();
        }

        public void PlayFromTimelines(int index)
        {
            TimelineAsset selectedAsset;            

            if (timelines.Count <= index)
            {
                selectedAsset = timelines[timelines.Count - 1];                
            }
            else
            {
                selectedAsset = timelines[index];
            }
            playableDirectors[0].Play(selectedAsset);
            CCanvasBoard.instance.closeCanvas();
                     
            Timing.RunCoroutine(CanvasClose());                           
        }
      
        private IEnumerator<float> CanvasClose()
        {
            float timelinesduration = (float)playableDirectors[0].duration;
            yield return Timing.WaitForSeconds(timelinesduration);
            Debug.Log("Show UI Canvas!!");
            CCanvasBoard.instance.showCanvas();
           
        }
    }
}
