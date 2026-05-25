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
        // NOVÝ ZÁPIS: Získa pozíciu myši na obrazovke
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 direction = (targetPoint - transform.position).normalized;
        direction.y = 0; 

        Vector3 spawnPos = transform.position + transform.forward * 0.8f + Vector3.up * 1.2f;
        GameObject vankus = Instantiate(vankusPrefab, spawnPos, Quaternion.LookRotation(direction));
        
        Rigidbody rb = vankus.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * shootingForce; // Už opravené na linearVelocity!
        }

        Destroy(vankus, 3f);
    }
}