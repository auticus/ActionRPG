using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Ray _targetRay;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateAnimator();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToClickPoint();
        }
    }

    private void MoveToClickPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hasHit = Physics.Raycast(ray, out var hit);
        if (hasHit) 
            _navMeshAgent.destination = hit.point;
    }

    private void UpdateAnimator()
    {
        var velocity = GetComponent<NavMeshAgent>().velocity;
        var localVelocity = transform.InverseTransformDirection(velocity); //transforms a world and turning into local space
        //when creating velocity we are storing here we are grabbing the global velocity and it may be X = 273 and Y = 7
        //thats not useful for the animator.  Animator just wants to know local velocity to say to animator you are moving forward at 3 units etc
        //global values are not as useful from local point of view

        var speed = localVelocity.z; //need to know the zed speed in a forward direction
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed); //ForwardSpeed is set on the animator Blend Tree
    }
}
