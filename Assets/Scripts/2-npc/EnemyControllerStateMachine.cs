using UnityEngine;

/**
 * This component patrols between given points and chases a given target object when it sees it.
 */
[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(Chaser))]
public class EnemyControllerStateMachine : StateMachine
{
    [SerializeField] private float radiusToWatch = 5f;

    private Chaser chaser;
    private Patroller patroller;

    private float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, chaser.TargetObjectPosition());
    }

    private void Awake()
    {
        chaser = GetComponent<Chaser>();
        patroller = GetComponent<Patroller>();

        base
            .AddState(patroller)     // This would be the first active state.
            .AddState(chaser)
            .AddTransition(patroller, () => DistanceToTarget() <= radiusToWatch, chaser)
            .AddTransition(chaser, () => DistanceToTarget() > radiusToWatch, patroller);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusToWatch);
    }
}
