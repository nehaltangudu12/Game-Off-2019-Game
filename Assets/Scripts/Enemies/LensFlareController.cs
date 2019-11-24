using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlareController : MonoBehaviour
{
    private Vector3 startscale;
    public GameObject player;

    [SerializeField, Tooltip("The range at which the flare starts increasing.")]
    public float maxrange;

    [SerializeField, Tooltip("The rate at which the flare increases in size")]
    public float scalerate;


    // Start is called before the first frame update
    void Start()
    {
        startscale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance < maxrange)
        {
            gameObject.transform.localScale += new Vector3(((1/distance) * scalerate), ((1 / distance) * scalerate), ((1 / distance) * scalerate));
        }
        else
        {
            gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, startscale , ((0.1f) * scalerate));
        }
    }
}
