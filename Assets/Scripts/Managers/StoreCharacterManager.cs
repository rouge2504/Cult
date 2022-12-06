using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCharacterManager : MonoBehaviour
{
    public ScrollRect StoreCharacterScrollRect;
    private GameObject contentStoreCharacter;

    [SerializeField]
    private GameObject prefabCharacterItem;

    
    private List<NPCItem> items = new List<NPCItem>();
    // Start is called before the first frame update
    void Start()
    {
        contentStoreCharacter = StoreCharacterScrollRect.content.gameObject;

        //Instantiate(prefabCharacterItem, contentStoreCharacter.transform);


        for (int i = 0; i < NPCManager.instance.npcs.Length; i++)
        {
            
            GameObject item = Instantiate(prefabCharacterItem, contentStoreCharacter.transform);
            item.GetComponent<NPCItem>().nameText.text = NPCManager.instance.npcs[i].name;
            item.GetComponent<NPCItem>().descriptionText.text = NPCManager.instance.npcs[i].description;

            items.Add(item.GetComponent<NPCItem>());

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
