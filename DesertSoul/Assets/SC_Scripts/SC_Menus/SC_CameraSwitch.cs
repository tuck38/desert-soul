using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Yarn.Unity.Attributes;

[CanEditMultipleObjects]

public class SC_CameraSwitch : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D collider2D;

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            if(customInspectorObjects.panCam)
            {
                SC_CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDist, customInspectorObjects.panTime, 
                customInspectorObjects.panDir, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Vector2 exitDir = (col.transform.position - collider2D.bounds.center).normalized;

            if(customInspectorObjects.panCam)
            {
                SC_CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDist, customInspectorObjects.panTime, 
                customInspectorObjects.panDir, true);
            }

            if(customInspectorObjects.swapCam && customInspectorObjects.leftCam != null && customInspectorObjects.rightCam != null)
            {
                SC_CameraManager.instance.CameraSwitch(customInspectorObjects.leftCam, customInspectorObjects.rightCam, exitDir);
            }
        }
    }
}


[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCam = false;

    public bool panCam = false;

    [HideInInspector] public CinemachineCamera leftCam;

    [HideInInspector] public CinemachineCamera rightCam;

    [HideInInspector] public float panDist;
    [HideInInspector] public float panTime;

    [HideInInspector] public PanDir panDir;
}


public enum PanDir
{
    Up,
    Down,
    Left,
    Right
}

[CustomEditor(typeof(SC_CameraSwitch))]
public class MyScriptEditor : Editor
{

    SC_CameraSwitch camSwitch;

    private void OnEnable()
    {
        camSwitch = (SC_CameraSwitch)target;
    }

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        if(camSwitch.customInspectorObjects.swapCam)
        {
            camSwitch.customInspectorObjects.leftCam = EditorGUILayout.ObjectField("Camera on Left",
            camSwitch.customInspectorObjects.leftCam, typeof(CinemachineCamera), true) as CinemachineCamera;

            camSwitch.customInspectorObjects.rightCam = EditorGUILayout.ObjectField("Camera on Right", 
            camSwitch.customInspectorObjects.rightCam, typeof(CinemachineCamera), true) as CinemachineCamera;
        }

        if(camSwitch.customInspectorObjects.panCam)
        {
            camSwitch.customInspectorObjects.panDir = (PanDir)EditorGUILayout.EnumPopup("Camera Pan Direction",
            camSwitch.customInspectorObjects.panDir);

            camSwitch.customInspectorObjects.panDist = EditorGUILayout.FloatField("Camera Pan Distance", 
            camSwitch.customInspectorObjects.panDist);

            camSwitch.customInspectorObjects.panTime = EditorGUILayout.FloatField("Camera Pan Time", 
            camSwitch.customInspectorObjects.panTime);
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(camSwitch);
        }
    }
}