using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class PlayerController : CharacterBase
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera mainCam;
    [SerializeField] private const float mouseSensitivity = 175f;
    [SerializeField] private const float movemnetSpeed = 12f;
    private Transform barrelTransform;
    private float xRotation;
    private float maxHealth = 100f;
    private float currentHealth;
    public float rayDistance = 100f;    // Ray'in mesafesi
    public float maxDistance = 1000f;   // Ray'in çarpmadığı durumda merminin gideceği mesafe

    RaycastHit hitInfo;

    [SerializeField] private WeaponBase currentWeapon;
    [SerializeField] private GameObject bulletPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        barrelTransform = currentWeapon.GetBarreTrasnform();
        currentHealth = maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            LookAround();
            Move();

            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
            if (IsCameraRayHit() && DetermineIsWeapon() && Input.GetKeyDown(KeyCode.G))
            {
                PickUpWeapon(hitInfo.collider.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.Q) && currentWeapon != null)
            {
                DropWeapon();
            }
            if (Input.GetKeyDown(KeyCode.R) && currentWeapon)
            {
                currentWeapon.Reload();
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Attack()
    {
        if (currentWeapon)
        {
            currentWeapon.Shoot();

            if (mainCam == null) return;

            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            RaycastHit hit;
            Vector3 targetPoint = ray.origin + ray.direction * maxDistance;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                targetPoint = hit.point;
            }

            GameObject bulletGO = Instantiate(bulletPrefab, barrelTransform.transform.position, Quaternion.identity);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.SetTargetTransform(targetPoint); // Hedef noktayı merminin hedefi olarak ayarla
                bullet.SetDamage(currentWeapon.GetDamage());
            }
        }
    }

    public override void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xDirection + transform.forward * zDirection;
        controller.Move(move.normalized * (movemnetSpeed * Time.deltaTime));
    }

    public void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        
        mainCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    public override void Die()
    {
        isDead = true;
    }

    public override void TakeDamage(float damage)
    {
        if (isDead) { return; }
        
        currentHealth -= damage;

        Debug.Log("health : " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void DropWeapon()
    {
        currentWeapon.transform.SetParent(null);

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = currentWeapon.gameObject.AddComponent<Rigidbody>();
        }

        rb.AddForce(transform.forward * 10, ForceMode.Impulse);

        currentWeapon = null;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void PickUpWeapon(GameObject weapon)
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }

        currentWeapon = DetermineWeaponType(weapon);
        currentWeapon.transform.SetParent(weaponHolder);
        
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        barrelTransform = currentWeapon.GetBarreTrasnform();
        
        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(rb);
        }
    }

    public bool IsCameraRayHit()
    {
        Ray cameraRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
        return Physics.Raycast(cameraRay, out hitInfo);
    }

    private WeaponBase DetermineWeaponType(GameObject weapon)
    {
        switch (weapon.tag)
        {
            case "Pistol": return weapon.GetComponent<Pistol>();
            case "MachineGun": return weapon.GetComponent<MachineGun>();;
            case "Bazooka": return weapon.GetComponent<Bazooka>();;
        }

        return null;
    }

    private bool DetermineIsWeapon()
    {
        if (hitInfo.collider.gameObject == null) { return false; }
        
        return hitInfo.collider.gameObject.CompareTag("Pistol") ||
               hitInfo.collider.gameObject.CompareTag("MachineGun") ||
               hitInfo.collider.gameObject.CompareTag("Bazooka");
    }
}
