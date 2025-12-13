using Unity.Cinemachine;
using UnityEngine;

public class SC_CameraManager : MonoBehaviour
{
    public static SC_CameraManager instance;

    [SerializeField] private CinemachineCamera[] allCams;

    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallPanTime = 0.25f;
    [SerializeField] private float fallPanChangeTreshold = -15f;

    public bool IsLerpingYDampening { get; private set; }

    public bool LerpedFromPlayerFalling { get; private set; }

    private Coroutine lerpYPanCoroutine;

    private CinemachineCamera currentCam;
    private CinemachinePositionComposer framingTransposer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for(int i = 0; i < allCams.Length; i++)
        {
            if (allCams[i].enabled)
            {
                currentCam = allCams[i];

                currentCam.GetCinemachineComponent(framingTransposer.Stage);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
