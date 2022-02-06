using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    public int[] inventory = new int[Enum.GetNames(typeof(Utils.InventoryObjectType)).Length];

    private GameObject inventoryVisualizer;


    void Start()
    {
        inventory[(int)Utils.InventoryObjectType.Water] = 0;
        inventory[(int)Utils.InventoryObjectType.Food] = 0;
        inventory[(int)Utils.InventoryObjectType.Wood] = 0;
        inventory[(int)Utils.InventoryObjectType.Nails] = 0;
    }

    public void Add(Utils.InventoryObjectType objectType)
    {
        Debug.Log("Adding " + objectType);
        switch (objectType)
        {
            case Utils.InventoryObjectType.Water:
                {
                    AddWater();
                    break;
                }
            case Utils.InventoryObjectType.Food:
                {
                    AddFood();
                    break;
                }
            case Utils.InventoryObjectType.Wood:
                {
                    AddWood();
                    break;
                }
            case Utils.InventoryObjectType.Nails:
                {
                    AddNails();
                    break;
                }

            default:
                break;
        }
    }

    public void Remove(Utils.InventoryObjectType objectType)
    {
        GenericRemoval((int)objectType);
    }

    public void SetVisualizer(GameObject visualizer)
    {
        this.inventoryVisualizer = visualizer;
    }

    public int Get(Utils.InventoryObjectType objectType)
    {
        switch (objectType)
        {
            case Utils.InventoryObjectType.Water:
                {
                    return inventory[Utils.waterInventoryIndex];
                }
            case Utils.InventoryObjectType.Food:
                {

                    return inventory[Utils.foodInventoryIndex];
                }
            case Utils.InventoryObjectType.Wood:
                {

                    return inventory[Utils.woodInventoryIndex];
                }
            case Utils.InventoryObjectType.Nails:
                {

                    return inventory[Utils.nailsInventoryIndex];
                }
            default:
                return 0;
        }
    }

    private void GenericRemoval(int index)
    {
        if (inventory[index] != 0)
        {
            inventory[index]--;
        }

        if (inventory[index] == 0)
        {
            Debug.Log("Water a = 100");
            inventoryVisualizer.transform.GetChild(index).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        }
    }

    private void AddNails()
    {
        if (inventory[Utils.nailsInventoryIndex] == 0)
        {
            inventory[Utils.nailsInventoryIndex]++;
            inventoryVisualizer.transform.parent.gameObject.GetComponent<AudioSource>().Play();
            inventoryVisualizer.transform.GetChild(Utils.nailsInventoryIndex)
                .gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
    }

    private void AddWood()
    {
        if (inventory[Utils.woodInventoryIndex] == 0)
        {
            inventory[Utils.woodInventoryIndex]++;
            inventoryVisualizer.transform.parent.gameObject.GetComponent<AudioSource>().Play();
            inventoryVisualizer.transform.GetChild(Utils.woodInventoryIndex)
                .gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
    }

    private void AddFood()
    {
        inventory[Utils.foodInventoryIndex]++;
        inventoryVisualizer.transform.parent.gameObject.GetComponent<AudioSource>().Play();
        inventoryVisualizer.transform.GetChild(Utils.foodInventoryIndex)
                .gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

    private void AddWater()
    {
        if (inventory[Utils.waterInventoryIndex] == 0)
        {
            inventory[Utils.waterInventoryIndex]++;
            inventoryVisualizer.transform.parent.gameObject.GetComponent<AudioSource>().Play();
            inventoryVisualizer.transform.GetChild(Utils.waterInventoryIndex)
                .gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
    }
}
