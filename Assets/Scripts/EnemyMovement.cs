using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody _rigidbody;
    private EnemyAI _playerAwarenessController;
    private Vector2 _targetDirection;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerAwarenessController = GetComponent<EnemyAI>();
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection(); // 1. Zistíme, kam máme ísť
        RotateTowardsTarget();   // 2. Otočíme sa tam
        SetVelocity();           // 3. Dodáme silu/rýchlosť na pohyb
    }

    private void UpdateTargetDirection()
    {
        // Ak skript PlayerAwarenessController hlási, že vidí hráča...
        if (_playerAwarenessController.AwareOfPlayer)
        {
            // ...nastavíme smer priamo na hráča
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else
        {
            // ...ak o ňom nevie, smer je nulový (nepriateľ nikam nechce ísť)
            _targetDirection = Vector2.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        // Ak stojíme na mieste (smer je nula), neotáčame sa a ukončíme funkciu
        if (_targetDirection == Vector2.zero)
        {
            return;
        }

        // Vytvoríme cieľovú rotáciu pre 2D priestor. 
        // transform.forward udržiava objekt v správnej osi a _targetDirection ho natáča za hráčom
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);

        // Plynule posunieme aktuálnu rotáciu smerom k cieľovej rotácii na základe rýchlosti otáčania
        // Použitie Time.deltaTime (resp. Time.fixedDeltaTime vo FixedUpdate) zabezpečí plynulosť bez ohľadu na FPS
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        // Aplikujeme novú rotáciu na Rigidbody
        _rigidbody.MoveRotation(rotation);
    }

    private void SetVelocity()
    {
        // Ak nemáme žiadny cieľ, zastavíme fyzikálny pohyb
        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            // Ak máme cieľ, pohneme nepriateľom smerom "hore" (v 2D hrách je transform.up smer, kam sa objekt díva)
            // a vynásobíme to nastavenou rýchlosťou
            _rigidbody.linearVelocity = transform.up * _speed;
        }
    }
}
