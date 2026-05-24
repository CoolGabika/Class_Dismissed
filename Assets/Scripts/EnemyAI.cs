using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private float _playerAwarenessDistance = 15f; // Na koľko metrov ťa učiteľ zbadá

    // Tieto premenné môžu v budúcnosti čítať iné skripty (napr. pre útok)
    public bool AwareOfPlayer { get; private set; }
    public Vector3 DirectionToPlayer { get; private set; }

    private Transform _player;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        // Natvrdo nastavenie agenta pre plynulý beh v tlupe
        if (_agent != null)
        {
            _agent.speed = 3.5f;
            _agent.stoppingDistance = 0f;
            _agent.acceleration = 15f;
            
            // Ignorovanie tancovania/vyhýbania sa medzi zombíkmi navzájom
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        // Automaticky nájde objekt "Ziak" podľa mena
        GameObject playerObj = GameObject.Find("Ziak");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
    }

    void Update()
    {
        if (_player == null || _agent == null) return;

        // 1. MATEMATIKA SENZORU (To, čo robil PlayerAwarenessController)
        Vector3 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        // Kontrola vzdialenosti
        if (enemyToPlayerVector.magnitude <= _playerAwarenessDistance)
        {
            AwareOfPlayer = true;
        }
        else
        {
            AwareOfPlayer = false;
        }

        // 2. POHYB PODĽA SENZORU
        if (AwareOfPlayer)
        {
            // Ak ťa vidí, zapne NavMesh a uteká za tebou kamkoľvek ideš
            _agent.isStopped = false;
            _agent.SetDestination(_player.position);
        }
        else
        {
            // Ak si ďaleko, okamžite zastaví na mieste a čaká
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }
    }
}