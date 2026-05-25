using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _playerAwarenessDistance = 25f; // Na koľko metrov ťa zbadá
    [SerializeField] private float _attackDistance = 2.2f;         // Vzdialenosť, na ktorej zastaví a začne ťa damageovať

    [SerializeField] private float damageAmount = 10f;  
    [SerializeField] private float attackSpeed = 1f;    // Útok každú 1 sekundu

    public bool AwareOfPlayer { get; private set; }
    public Vector3 DirectionToPlayer { get; private set; }

    private Transform _player;
    private NavMeshAgent _agent;
    private PlayerHealth _playerHealth; 
    private float _attackTimer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_agent != null)
        {
            _agent.speed = 5f;              // Zvýšil som rýchlosť, aby bežali agresívnejšie
            _agent.stoppingDistance = 0f;   // V kóde si to riadime sami, takže agenta necháme na 0
            _agent.acceleration = 30f;      // Okamžitý štart bez spomaľovania
            _agent.autoBraking = false;     // Zakázané brzdenie pred cieľom
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        GameObject playerObj = GameObject.Find("Ziak");
        if (playerObj != null)
        {
            _player = playerObj.transform;
            _playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (_player == null || _agent == null) return;

        // Vypočítame presnú vzdialenosť medzi zombíkom a Žiakom
        Vector3 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;
        float distanceToPlayer = enemyToPlayerVector.magnitude;

        AwareOfPlayer = distanceToPlayer <= _playerAwarenessDistance;

        if (AwareOfPlayer)
        {
            // --- KĽÚČOVÁ ZMENA: AK JE DOSŤ BLÍZKO (napr. pod 2.2 metra) ---
            if (distanceToPlayer <= _attackDistance)
            {
                // OKAMŽITE MU ZAMRAZÍME MOTOR - nezníži plyn, proste stopne na mieste mimo teba
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;

                // Stále sa otáča za tebou, ak by si ho obchádzala
                Vector3 lookDir = _player.position - transform.position;
                lookDir.y = 0;
                if (lookDir != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 15f);
                }

                // Spustenie damageovania
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= attackSpeed)
                {
                    AttackPlayer();
                    _attackTimer = 0f; 
                }
            }
            else
            {
                // --- AK JE ĎALEJ AKO 2.2 metra -> ŠPRINTUJE NAPLNO ---
                _agent.isStopped = false;
                _agent.SetDestination(_player.position);
                _attackTimer = 0f; // Reset útoku, ak hráč utiekol z dosahu
            }
        }
        else
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }
    }

    void AttackPlayer()
    {
        if (_playerHealth != null)
        {
            _playerHealth.TakeDamage(damageAmount);
        }
    }
}