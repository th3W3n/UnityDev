using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent agent;

    [SerializeField] private AudioClip loseClip;
    private AudioSource source;

    private Vector3 initPos;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        initPos = transform.position;
    }

    private void Update()
    {
        if (player != null)
            agent.SetDestination(player.position);
    }

    public void PlayGameLose() => source.PlayOneShot(loseClip);
    public void ResetState()
    {
        gameObject.SetActive(true);
        agent.Warp(initPos);
    }
}