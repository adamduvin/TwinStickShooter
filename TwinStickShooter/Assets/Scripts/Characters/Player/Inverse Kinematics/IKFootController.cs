using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    private PlayerMovement playerMovement;
    [SerializeField]
    private GameObject mainBody;
    [SerializeField]
    private GameObject bodyToAboveFoot;
    private LayerMask groundMask;
    [SerializeField]
    private IKFootMaster iKFootMaster;
    private float stepDistance;
    private float stepHeight;
    private float speed;
    private Vector3 currentPosition;
    private Vector3 newPosition;
    private Vector3 oldPosition;
    private float lerp;

    [SerializeField]
    private GameObject partnerFoot;

    [SerializeField]
    private bool isPlanted;

    public bool IsPlanted
    {
        set { isPlanted = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        stepDistance = iKFootMaster.StepDistance;
        stepHeight = iKFootMaster.StepHeight;
        speed = iKFootMaster.Speed;

        groundMask = LayerMask.GetMask("Ground");
        /*bodyToAboveFoot = transform.position - mainBody.transform.position;
        bodyToAboveFoot.y = mainBody.transform.position.y;*/
        currentPosition = newPosition = oldPosition = transform.position;
        lerp = 1;

        playerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = currentPosition;

        /*bodyToAboveFoot = transform.position;
        bodyToAboveFoot.y = mainBody.transform.position.y;*/
        Ray ray = new Ray(bodyToAboveFoot.transform.position, Vector3.down);
        if(!isPlanted && Physics.Raycast(ray, out RaycastHit info, 10, groundMask.value) && lerp >= 1f)
        {
            if(Vector3.Distance(newPosition, info.point) > stepDistance)
            {
                lerp = 0;
                newPosition = info.point;
            }
        }
        if(lerp < 1)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * playerMovement.Controller.velocity.magnitude * speed;
            if(lerp >= 1)
            {
                isPlanted = true;
                partnerFoot.GetComponent<IKFootController>().IsPlanted = false;
            }
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }
}
