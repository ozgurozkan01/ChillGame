using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class PlayerController : CharacterBase
{
    [SerializeField] private WeaponType currentWeaponType;
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
    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject machineGunPrefab;
    [SerializeField] private GameObject bazookaPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private InventoryController inventory;
    
    
    // Start is called before the first frame update
    void Start()
    {
        barrelTransform = currentWeapon.GetBarreTrasnform();
        currentHealth = maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetInitialGun();
        inventory.AddWeapon(currentWeapon.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            SwitchEquippedWeapon();
            LookAround();
            Move();

            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
            if (IsCameraRayHit() && DetermineIsWeapon() && Input.GetKeyDown(KeyCode.G) && hitInfo.collider.gameObject != null)
            {
                if (inventory.IsEnvanterFull())
                {
                    DropWeapon();
                    inventory.ReplaceCurrentWeapon(hitInfo.collider.gameObject);
                }
                else
                {
                    PickUpWeapon(hitInfo.collider.gameObject);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Q) && currentWeapon != null)
            {
                DropWeapon();
                inventory.emptySlotAmount--;
            }
            if (Input.GetKeyDown(KeyCode.R) && currentWeapon)
            {
                currentWeapon.Reload();
            }
        }
    }

    public void SetInitialGun()
    {
        GameObject weapon = null;
        switch (currentWeaponType)
        {
            case WeaponType.Pistol: weapon = Instantiate(pistolPrefab, weaponHolder.position, weaponHolder.rotation); break;
            case WeaponType.MachineGun: weapon = Instantiate(machineGunPrefab, weaponHolder.position, weaponHolder.rotation); break;
            case WeaponType.Bazooka: weapon = Instantiate(bazookaPrefab, weaponHolder.position, weaponHolder.rotation); break;
        }
        
        weapon.transform.Rotate(Vector3.up, 90);
        weapon.transform.SetParent(weaponHolder);
        currentWeapon = weapon.GetComponent<WeaponBase>();
        barrelTransform = weapon.GetComponent<WeaponBase>().GetBarreTrasnform();
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
        if (currentWeapon == null) { return; }
    
        // Silahı envanterden kaldır
        inventory.RemoveWeapon(currentWeapon.gameObject);
        
        // Silahın ebeveynini kaldır
        currentWeapon.transform.SetParent(null);

        // Silaha Rigidbody ekle (eğer mevcut değilse)
        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = currentWeapon.gameObject.AddComponent<Rigidbody>();
        }

        // Silaha kuvvet ekle
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);

        // Silahı null yap
        currentWeapon = null;
    }


    // ReSharper disable Unity.PerformanceAnalysis
    public void PickUpWeapon(GameObject weapon)
    {
        inventory.AddWeapon(weapon);
        UpdateWeaponProperties(weapon);
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

    // ReSharper disable Unity.PerformanceAnalysis
    private void SwitchEquippedWeapon()
    {
        if (inventory == null) { return; }

        GameObject weapon = null;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = inventory.SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = inventory.SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = inventory.SwitchWeapon(2);
        }

        if (weapon != null)
        {
            UpdateWeaponProperties(weapon);
        }
    }

    void UpdateWeaponProperties(GameObject newWeapon)
    {
        newWeapon.transform.SetParent(weaponHolder);
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.Rotate(Vector3.up, 90);
        newWeapon.transform.localPosition = Vector3.zero;
        currentWeapon = DetermineWeaponType(newWeapon);
        barrelTransform = currentWeapon.GetBarreTrasnform();   

    }
}
