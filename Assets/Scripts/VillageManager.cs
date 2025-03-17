using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class VillageManager : MonoBehaviour
{
    public InputActionReference PressAction;
    public LayerMask ClickMask;
    // Start is called before the first frame update
    void Start()
    {
        SaveLoadManager.instance.Load();
        if (SaveLoadManager.instance.CheckIfSaveExists())
        {
            Debug.Log("Greetings! You last played at: ");
            SaveLoadManager.instance._GameData.SaveTime.WhatTime();
        }
        PressAction.action.performed += Clicked;

        IFormatProvider provider = CultureInfo.InvariantCulture;

        if (SaveLoadManager.instance._GameData.SaveTime != null)
        {
            DateTime ParsedSaveTime = DateTime.Parse(CustomTime.ReturnParseableTime(SaveLoadManager.instance._GameData.SaveTime));
            print("Attempting to parse: " + ParsedSaveTime.TimeOfDay);
        }
    }

    public void Clicked(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity, ClickMask);

        if (hasHit)
        {
            IInteractable test = hit.collider.gameObject.GetComponent<IInteractable>();
            if (test != null)
            {
                print("TAPPED INTERACTIBLE");
                test.Interact();
            }
        }
    }

    void GoToCombatScene()
    {
    
    }

    void OpenCraftingMenu()
    {
    
    }

    private void OnApplicationQuit()
    {
        SaveLoadManager.instance.Save();
    }
}
