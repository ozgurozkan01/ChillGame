using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject[] slots = new GameObject[3];
    private int currentWeaponIndex = -1;

    public void AddWeapon(GameObject weapon)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = weapon;
                EquipWeapon(i);
                return;
            }
        }
        ReplaceCurrentWeapon(weapon);
    }

    private GameObject EquipWeapon(int index)
    {
        if (currentWeaponIndex != -1)
        {
            slots[currentWeaponIndex].SetActive(false);
        }
        currentWeaponIndex = index;
        slots[currentWeaponIndex].SetActive(true);
        return slots[currentWeaponIndex];
    }

    private void ReplaceCurrentWeapon(GameObject newWeapon)
    {
        if (currentWeaponIndex != -1)
        {
            slots[currentWeaponIndex] = newWeapon;
            slots[currentWeaponIndex].SetActive(true);
        }
    }

    public GameObject SwitchWeapon(int index)
    {
        if (currentWeaponIndex != index && index >= 0 && index < slots.Length && slots[index] != null)
        {
            return EquipWeapon(index);
        }

        return null;
    }
}
