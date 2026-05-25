using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Táto funkcia sa spustí sama, keďže sme na kolidéri zaškrtli "Is Trigger"
    private void OnTriggerEnter(Collider other)
    {
        // Ak vankúš narazí do samotného Hráča (Žiaka), ignoruje to, aby neublížil sám sebe
        if (other.CompareTag("Player")) return;

        // Skontroluje, či objekt, do ktorého vankúš narazil, má na sebe skript EnemyHealth (učiteľa)
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(); // Spustí funkciu pre poškodenie/smrť učiteľa
        }

        // Vankúš sa po náraze do hocičoho (učiteľ, stena) okamžite zničí a zmizne
        Destroy(gameObject);
    }
}