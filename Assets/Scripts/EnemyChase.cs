using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float catchDistance = 2.5f;

    private bool caught = false;
    private Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (animator != null)
        {
            animator.SetBool("Caught", false);
        }

        if (agent != null)
        {
            agent.speed = speed;
            agent.stoppingDistance = catchDistance * 0.5f;
        }
    }

    void Update()
    {
        if (player == null || caught || agent == null) return;

        agent.SetDestination(player.position);

        Vector3 direction = agent.velocity;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= catchDistance)
        {
            caught = true;
            agent.isStopped = true;

            if (animator != null)
            {
                animator.SetBool("Caught", true);
            }

            Debug.Log("GAME OVER - Enemy caught the player");
        }
    }

    void OnGUI()
    {
        if (!caught) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 80;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.red;

        GUI.Label(
            new Rect(0, 0, Screen.width, Screen.height),
            "GAME OVER\nYOU WERE CAUGHT",
            style
        );
    }
}