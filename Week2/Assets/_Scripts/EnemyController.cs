using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent agent;

    [SerializeField] private AudioClip loseClip;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player != null)
            agent.SetDestination(player.position);
    }

    public void PlayGameLose() => source.PlayOneShot(loseClip);
}