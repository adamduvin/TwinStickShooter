using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootMaster : MonoBehaviour
{
    [SerializeField]
    private float stepDistance = 1f;
    public float StepDistance
    {
        get { return stepDistance; }
    }

    [SerializeField]
    private float stepHeight = 1f;
    public float StepHeight
    {
        get { return stepHeight; }
    }

    [SerializeField]
    private float speed = 1f;
    public float Speed
    {
        get { return speed; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
