using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MEC;
using mARTians;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace mARTians
{
    public enum CameraState
    {
        IDLE,
        PLAY,
        TIMELINE
    }

    public enum CameraFXState
    {
        IDLE,
        SHAKING
    }

    public enum CollisionImpactCalculatorState
    {
        IDLE,
        PROGRESS
    }
    public class CCameraController : MonoBehaviour
    {
        //! Handling All Camera Action and Production.

        [Header("The mARTians Camera Engine v0.1b")]
        [TextArea]
        [SerializeField]
        private string info = "Camera Engine Handling all Camera Motions, Effect and Post Processing. \n";

        //* The Main Camera
        private Camera mainCamera;
        [SerializeField]
        private CinemachineBrain cameraBrain;
        private Vector3 rawPosition;

        //* Child Cameras
        [Header("Camera and CinemaChine Production")]
        [SerializeField]
        private CinemachineFreeLook freelookCamera; //* Get Set Ready...
        [SerializeField]
        private CTimelineController timelineController;

        //* Camera Shaking FX
        [Header("FX, Camera Shake")]
        [SerializeField]
        private float cameraFXDamping = 2.0f;
        public float ShakingAmplitude = 100.0f;

        private CinemachineBasicMultiChannelPerlin topNoise;
        private CinemachineBasicMultiChannelPerlin middleNoise;
        private CinemachineBasicMultiChannelPerlin bottomNoise;

        public NoiseSettings telephotoNoiseProfile;
        public NoiseSettings handheldNoiseProfile;

        //* State
        [Header("Camera State")]
        public CameraState CameraState = CameraState.PLAY;
        public CameraFXState CameraFXState = CameraFXState.IDLE;
        public CollisionImpactCalculatorState CollisionImpactCalculatorState = CollisionImpactCalculatorState.IDLE;

        private void Awake()
        {
#if UNITY_EDITOR
            Debug.Log("The mARTians Camera Engine v0.1b Starting");
#endif      

            mainCamera = Camera.main;

            if (!cameraBrain)
            {
                Debug.Log("Cmaera Brain is Empty, Auto Assign Camera Brain");
                cameraBrain = GetComponentInChildren<CinemachineBrain>();
            }

            if (!freelookCamera)
            {
                Debug.Log("Camera Brain is Empty, Auto Assign Freelook Camera");
                freelookCamera = GetComponentInChildren<CinemachineFreeLook>();
            }

            if (!timelineController)
            {
                Debug.Log("Timeline Controller is Empty, Auto Assign Timeline Controller");
                timelineController = GetComponentInChildren<CTimelineController>();
            }

            //* get set ready. . .    
            // if (!topNoise || !middleNoise || !bottomNoise)
            // {
            //     Debug.Log("Freelook Camera Noise is not Assigned, Auto Assign Camera's Noises");
            //     topNoise = freelookCamera.GetRig(0).AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //     middleNoise = freelookCamera.GetRig(1).AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //     bottomNoise = freelookCamera.GetRig(2).AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            // }

            // Profile Setup
            // topNoise.m_NoiseProfile = telephotoNoiseProfile;
            // middleNoise.m_NoiseProfile = telephotoNoiseProfile;
            // bottomNoise.m_NoiseProfile = handheldNoiseProfile;

            // Frequency Setup
            // topNoise.m_FrequencyGain = 0.15f;
            // middleNoise.m_AmplitudeGain = 0.15f;
            // bottomNoise.m_FrequencyGain = 0.2f;            

        }


    }
}