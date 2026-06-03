using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject efektCastic; // Sem v Unity pretiahneme náš modrý Prefab partiklov
    private bool _isDead = false;

    public void TakeDamage()
    {
        if (_isDead) return;
        Die();
    }

    void Die()
    {
        _isDead = true;
        Debug.Log(name + " bol vyradený a vymazaný!");

        // 1. Pripočítame kill do GameManageru
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddKill();
        }

        // 2. Vytvoríme efekt partiklov na mieste, kde stál učiteľ
        if (efektCastic != null)
        {
            GameObject particleInstance = Instantiate(efektCastic, transform.position + Vector3.up * 1f, Quaternion.identity);
            Destroy(particleInstance, 1f); // Vymaže samotné partikle z pamäte po 1 sekunde
        }

        // 3. Okamžite vymažeme učiteľa zo scény, aby nekrúžil a nebugoval sa
        Destroy(gameObject);
    }
}