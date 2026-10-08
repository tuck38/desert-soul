using Unity.Cinemachine;
using UnityEngine;
using System;
using System.Collections.Generic;
using NUnit.Framework;

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

    Transform activeTarget;

    private float normYPanAmnt;

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
            }
        }
    }

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

        isLerpingFromFall = false;
        isLerpingY = false;
    }

    public void CameraSwitch(CinemachineCamera camFromLeft, CinemachineCamera camFromRight, Vector2 triggerExitDir)
    {
        
    }
}