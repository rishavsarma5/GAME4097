using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadUI : MonoBehaviour
{
	public GameObject[] RowCollection;

	private GameObject[][] InventorySpaces;


    void Start()
    {
		GameObject[] itemsList;
		for (int row = 0; row < RowCollection.Length; row++)
		{
			itemsList = RowCollection[row].GetComponentsInChildren<GameObject>();
			for (int item = 0; item < 3; item++)
			{
				InventorySpaces[row][item] = itemsList[item];
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
