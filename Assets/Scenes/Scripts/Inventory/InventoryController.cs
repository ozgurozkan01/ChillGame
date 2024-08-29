using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject[] slots = new GameObject[3];
    private int currentWeaponIndex = -1;
    public float emptySlotAmount = 3;

    public void AddWeapon(GameObject weapon)
    {
        // Eğer envanterde boş yer varsa, yeni silahı ekle
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = weapon;
                EquipWeapon(i);
                emptySlotAmount++;
                return;
            }
        }
    }

    public bool IsEnvanterFull()
    {
        return emptySlotAmount == 0;
    }
    
    private GameObject EquipWeapon(int index)
    {
        // Geçerli index kontrolü
        if (index < 0 || index >= slots.Length) return null;

        // Mevcut silahı devre dışı bırak
        if (currentWeaponIndex != -1)
        {
            DeactivateWeapon(currentWeaponIndex);
        }

        // Yeni silahı etkinleştir
        currentWeaponIndex = index;
        ActivateWeapon(currentWeaponIndex);

        return slots[currentWeaponIndex];
    }

    public void ReplaceCurrentWeapon(GameObject newWeapon)
    {
        // Mevcut silah varsa devre dışı bırak
        if (currentWeaponIndex != -1)
        {
            DeactivateWeapon(currentWeaponIndex);
        }

        // Yeni silahı mevcut silahın yerine ekle
        if (currentWeaponIndex != -1)
        {
            slots[currentWeaponIndex] = newWeapon;
            EquipWeapon(currentWeaponIndex);
        }
    }

    public GameObject SwitchWeapon(int index)
    {
        if (index >= 0 && index < slots.Length && slots[index] != null && currentWeaponIndex != index)
        {
            return EquipWeapon(index);
        }

        return null;
    }

    private void ActivateWeapon(int index)
    {
        // Silahın gerçekten mevcut olduğunu kontrol et
        if (index < 0 || index >= slots.Length || slots[index] == null) return;

        slots[index].SetActive(true);

        Collider collider = slots[index].GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true; // Collider'ı etkinleştir
        }

        Rigidbody rb = slots[index].GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Rigidbody'yi kinematic olmaktan çıkar
        }
    }

    private void DeactivateWeapon(int index)
    {
        // Silahın gerçekten mevcut olduğunu kontrol et
        if (index < 0 || index >= slots.Length || slots[index] == null) return;

        slots[index].SetActive(false);

        Collider collider = slots[index].GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // Collider'ı devre dışı bırak
        }

        Rigidbody rb = slots[index].GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Rigidbody'yi kinematic yap
        }
    }

    public void RemoveWeapon(GameObject weapon)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == weapon)
            {
                slots[i] = null;

                // Eğer mevcut silah bırakıldıysa, currentWeaponIndex'i güncelle
                if (currentWeaponIndex == i)
                {
                    currentWeaponIndex = FindLowestIndexWithWeapon();
                    if (currentWeaponIndex != -1)
                    {
                        EquipWeapon(currentWeaponIndex);
                    }
                }

                // Silahın collider'ını ve rigidbody'sini yeniden ayarla
                DeactivateWeapon(i);
                return;
            }
        }
    }

    private int FindLowestIndexWithWeapon()
    {
        int lowestIndex = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                if (lowestIndex == -1 || i < lowestIndex)
                {
                    lowestIndex = i;
                }
            }
        }
        return lowestIndex;
    }
}
