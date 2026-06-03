using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _hasHit = false; // Poistka proti hromadnému zásahu

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        
        // Ak už vankúš niekoho trafil, ignoruje ďalšie kolízie
        if (_hasHit) return;

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            _hasHit = true; // Označíme, že vankúš už trafil cieľ

            // Vypneme kolíder vankúša, aby už fyzicky neexistoval pre ostatných
            Collider myCollider = GetComponent<Collider>();
            if (myCollider != null) myCollider.enabled = false;

            enemyHealth.TakeDamage();
            
            // Okamžite zničíme vankúš
            Destroy(gameObject);
        }
    }
}