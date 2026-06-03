using UnityEngine;
using UnityEngine.InputSystem; // Dôležité pre nový Input System!

public class PlayerShooting : MonoBehaviour
{
    public GameObject vankusPrefab;    
    public float shootingForce = 20f;  
    public float fireRate = 0.4f;      
    
    private float _nextFireTime;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // NOVÝ ZÁPIS: Skontroluje, či bolo stlačené ľavé tlačidlo myši
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= _nextFireTime && Time.timeScale > 0)
        {
            Shoot();
            _nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
    // 1. Získame pozíciu myši (Nový Input System)
    Vector2 mousePosition = Mouse.current.position.ReadValue();
    Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
    
    // 2. Vytvoríme matematickú rovinu vo výške hrudníka hráča (Ziak)
    Plane playerPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y + 1.2f, 0));
    float rayDistance;
    Vector3 targetPoint = transform.position + transform.forward * 10f; // Záložný smer, ak by lúč minul

    // 3. Zistíme, kde presne lúč z myši pretne túto rovinu
    if (playerPlane.Raycast(ray, out rayDistance))
    {
        targetPoint = ray.GetPoint(rayDistance);
    }

    // 4. Vypočítame smer od Žiaka k bodu kliknutia
    Vector3 direction = (targetPoint - transform.position).normalized;
    direction.y = 0; // Chceme, aby vankúš letel rovno, nie hore/dole

    // 5. Pozícia zrodu (2 metre pred hráčom, 1.2 metra vysoko)
    Vector3 spawnPos = transform.position + transform.forward * 1.5f + Vector3.up * 1.2f;
    
    // 6. Vytvorenie vankúša otočeného v smere letu
    GameObject vankus = Instantiate(vankusPrefab, spawnPos, Quaternion.LookRotation(direction));
    
    // 7. Vystrelenie vankúša
    Rigidbody rb = vankus.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.linearVelocity = direction * shootingForce; 
    }

    Destroy(vankus, 3f);
    }
}