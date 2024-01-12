using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Vuforia
using Vuforia;

//# NOTE: this is the script about detection Object
public class TrackingObject : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    public bool is_detected = false;

    //@ 1.Add variables for processing Atack (naem, atk, def, hp)
    public string _name;
    public float _atk;
    public float _def;
    public float _hp;

    //@ 2.Add variables (can change Card Information of 3D text in realtime)
    public TextMesh obj_text_mesh;

    private void Start()
    {
        //@ 2.Initiate Card info Setting to 3D text
        obj_text_mesh.text = _name + "<color=green> \n HP : </color>" + "<color=yellow>" + _hp + "</color>";

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus,
    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
        newStatus == TrackableBehaviour.Status.TRACKED)
        {
            is_detected = true;
        }
        else
        {
            is_detected = false;
        }
    }
}
