using Unity.Cinemachine;
using UnityEngine;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Diagnostics;

public class SC_CameraManager : MonoBehaviour
{
/*This Script is for the camera to follow the player with sprite rotation, most likely will be deprecated when we figure 
out the new cinemachine*/

    public static SC_CameraManager instance {get; private set; }
    [SerializeField] CinemachineCamera[] allCams;
    [SerializeField] Transform player;
    [SerializeField] Vector3 camOffset;
    [SerializeField] CinemachineCamera currentCamera;

    [SerializeField]  CinemachinePositionComposer composer;
    [SerializeField] float defaultOrthographicSize;

    [SerializeField] float fallPanAmnt = 0.25f;

    [SerializeField] float fallPanTime = 0.35f;

    [SerializeField] public float fallDampChangeTresh = -15f; 

    public bool isLerpingY;

    public bool isLerpingFromFall;

    Coroutine lerpYCoroutine;
    Coroutine camPanCoroutine;

    Transform activeTarget;

    private float normYPanAmnt;

    Vector2 trackedObjOffset;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        for(int i = 0; i < allCams.Length; i++)
        {
            if(allCams[i].enabled)
            {
                currentCamera = allCams[i];

                composer = currentCamera.GetComponent<CinemachinePositionComposer>();

                normYPanAmnt = composer.Damping.y;
            }
        }

        trackedObjOffset = composer.TargetOffset;
    }

    #region fall dampening

    public void LerpYDampening(bool isPlayerFalling)
    {
        lerpYCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator<Type> LerpYAction(bool isPlayerFalling)
    {
        isLerpingY = true;

        float startDampAmnt = composer.Damping.y;
        float endDampAmnt = 0f;

        if(isPlayerFalling)
        {
            endDampAmnt = fallPanAmnt;
            isLerpingFromFall = true;
        }
        else
        {
            endDampAmnt = normYPanAmnt;
        }

        float elapsedTime = 0f;

        while(elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmnt = Mathf.Lerp(startDampAmnt, endDampAmnt, (elapsedTime / fallPanTime));

            composer.Damping.y = lerpedPanAmnt;

            yield return null;
        }

        isLerpingY = false;
    }

    #endregion

    #region camera panning
        
    public void PanCameraOnContact(float panDist, float panTime, PanDir panDir,  bool panToStart)
    {
        camPanCoroutine = StartCoroutine(PanCam(panDist, panTime, panDir, panToStart));
    }

    private IEnumerator<Type> PanCam(float panDist, float panTime, PanDir panDir,  bool panToStart)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if(!panToStart)
        {
            switch (panDir)
            {
                case PanDir.Up:
                    endPos = Vector2.up;
                    break;
                case PanDir.Down:
                    endPos = Vector2.down;
                    break;
                case PanDir.Left:
                    endPos = Vector2.left;
                    break;
                case PanDir.Right:
                    endPos = Vector2.right;
                    break;
            }

            endPos *= panDist;

            startingPos = trackedObjOffset;

            endPos += startingPos;
        }
        else
        {
            startingPos = composer.TargetOffset;

            endPos = trackedObjOffset;
        }

        float elapsedTime = 0f;

        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime/ panTime));

            composer.TargetOffset = panLerp;

            yield return null;
        }
    }

    #endregion

    #region camera switch
    //DOES NOT ACCOUNT FOR UP AND DOWN YET
    public void CameraSwitch(CinemachineCamera camFromLeft, CinemachineCamera camFromRight, Vector2 triggerExitDir, bool horizontal)
    {
        if(horizontal == true)
        {
            if(camFromLeft == currentCamera && triggerExitDir.x > 0f)
            {
                camFromRight.enabled = true;

                camFromLeft.enabled = false;

                currentCamera = camFromRight;

                composer = currentCamera.GetComponent<CinemachinePositionComposer>();
            }
            else if(camFromRight == currentCamera && triggerExitDir.x < 0f)
            {
                camFromLeft.enabled = true;

                camFromRight.enabled = false;

                currentCamera = camFromLeft;

                composer = currentCamera.GetComponent<CinemachinePositionComposer>();
            }
        }
        else if(horizontal == false)
        {
            if(camFromLeft == currentCamera && triggerExitDir.y > 0f)
            {
                UnityEngine.Debug.Log("up");
                camFromRight.enabled = true;

                camFromLeft.enabled = false;

                currentCamera = camFromRight;

                composer = currentCamera.GetComponent<CinemachinePositionComposer>();
            }
            else if(camFromRight == currentCamera && triggerExitDir.y < 0f)
            {
                UnityEngine.Debug.Log("down");
                camFromLeft.enabled = true;

                camFromRight.enabled = false;

                currentCamera = camFromLeft;

                composer = currentCamera.GetComponent<CinemachinePositionComposer>();
            }
        }
    }

    #endregion
}