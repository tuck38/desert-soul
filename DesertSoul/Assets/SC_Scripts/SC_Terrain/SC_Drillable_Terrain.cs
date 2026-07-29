using UnityEngine;

public class SC_Drillable_Terrain : MonoBehaviour
{
    [SerializeField] private ParticleSystem breakParticles;
    [SerializeField] private GameObject particleSyst;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask thisLayer;
    BoxCollider2D col;
    SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 origin = transform.position;
        Vector3 directionA = transform.forward;
        //Left
        if (Physics2D.Raycast(origin, directionA, rayDistance, thisLayer))
        {
            //Debug.Log($"Hit object on the LEFT: {col.name}");
            particleSyst.transform.Rotate(new Vector3(0,0,0));
            breakParticles.Play();
        }
        else
        {
            //Debug.Log("Drawing Ray, no hit");

        }

        //Right
        Vector3 directionB = -transform.forward;
        if (Physics2D.Raycast(origin, directionB, rayDistance, thisLayer))
        {
            //Debug.Log($"Hit object on the Right: {col.name}");
            particleSyst.transform.Rotate(new Vector3(180,0,0));
            breakParticles.Play();
        }
        else
        {
            //Debug.Log("Drawing Ray, no hit");

        }*/

    }

    public void BreakTerrain()
    {
        col.enabled = false;
        breakParticles.Play();
        Destroy(gameObject);
    }

    public void FixTerrain()
    {
        //Made this if we wanted to reset the terrain with day/night cycle
        col.enabled = true;
        sprite.enabled = true;
    }
}
