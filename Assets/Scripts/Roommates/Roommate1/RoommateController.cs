using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Sunbox.Avatars;

public class RoommateController : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private Animator animator;

    // HungerSlider
    public Slider playerHunger;

    public Transform fridgeTarget; // Target of the roommate when the player is hungry

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (TryGetComponent<AvatarCustomization>(out var avatar))
        {
            animator = avatar.Animator;
            if (animator != null)
            {
                animator.applyRootMotion = false;
            }
        }


        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState(this));
    }

    void Update()
    {
        stateMachine.Update();

        if (animator == null || animator.runtimeAnimatorController == null)
        {
            return;
        }

        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        Vector2 movement = agent.speed > 0f
            ? Vector2.ClampMagnitude(new Vector2(localVelocity.x, localVelocity.z) / agent.speed, 1f)
            : Vector2.zero;

        animator.SetFloat(MoveX, movement.x);
        animator.SetFloat(MoveY, movement.y);
    }

    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

    // Moves the roommate towards the target
    public void MoveTo(Transform target)
    {
        agent.SetDestination(target.position);
    }

    // Checks if the roommate has reached the target
    public bool HasArrived()
    {
        return !agent.pathPending &&agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f;
    }

    // Stops the roommate's movement
    public void StopMoving()
    {
        agent.ResetPath();
    }
}