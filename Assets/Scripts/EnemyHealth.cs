using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    // Funkcia, ktorú zavolá letiaci vankúš pri náraze
    public void TakeDamage()
    {
        Die();
    }

    void Die()
    {
        Debug.Log(name + " dostal vankúšom a bol vyradený!");

        // Vypne NavMeshAgenta, aby sa učiteľ po smrti hneď prestal hýbať a prenasledovať ťa
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        // Vypne skript EnemyAI (tvoj pohybový kód učiteľa), aby sa nehádal s vypnutým agentom
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        // EFEKT SMRTI: Zrotuje celého učiteľa o -90 stupňov na X osi (odpadne dozadu na chrbát)
        transform.rotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, 0);

        // Vymaže telo učiteľa zo scény po 4 sekundách, aby nezavadzalo na zemi
        Destroy(gameObject, 4f);
    }
}
