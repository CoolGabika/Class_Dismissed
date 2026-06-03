using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private float _rotationSpeed = 120f;

    private Rigidbody _rigidbody;
    private EnemyAI _playerAwarenessController;
    private Vector3 _targetDirection; // ZMENA: V 3D svete používame Vector3!

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerAwarenessController = GetComponent<EnemyAI>();
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection(); // 1. Zistíme, kam máme ísť
        RotateTowardsTarget();   // 2. Otočíme sa tam
        SetVelocity();           // 3. Pohneme sa tam
    }

   private void UpdateTargetDirection()
    {
    // ÚPРАVА: Úplne ignorujeme, či o nás EnemyAI vie, a hľadáme hráča natvrdo cez celú mapu!
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _targetDirection = (player.transform.position - transform.position).normalized;
            _targetDirection.y = 0; // Držíme ich na zemi
        }
        else
        {
            _targetDirection = Vector3.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector3.zero)
        {
            return;
        }

        // ZMENA: Správna 3D rotácia smerom k cieľu
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
        
        // Plynulé otáčanie v 3D
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

        _rigidbody.MoveRotation(rotation);
    }

    private void SetVelocity()
    {
        if (_targetDirection == Vector3.zero)
        {
            // Zastavenie v 3D
            _rigidbody.linearVelocity = Vector3.zero;
        }
        else
        {
            // ZMENA: V 3D hrách je smer dopredu transform.forward (nie transform.up)!
            // Tlačíme učiteľa v smere, kam sa reálne pozerá
            _rigidbody.linearVelocity = transform.forward * _speed;
        }
    }
}