using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCharacterManager : MonoBehaviour
{
    public ScrollRect StoreCharacterScrollRect;
    private GameObject contentStoreCharacter;

    [SerializeField]
    private GameObject prefabCharacterItemButton;
    [SerializeField]
    private GameObject[] charactersPrefabs;

    
    private List<NPCItem> items = new List<NPCItem>();
    // Start is called before the first frame update
    void Start()
    {
        contentStoreCharacter = StoreCharacterScrollRect.content.gameObject;

        //Instantiate(prefabCharacterItem, contentStoreCharacter.transform);


        for (int i = 0; i < NPCManager.instance.npcs.Length; i++)
        {
            
            GameObject item = Instantiate(prefabCharacterItemButton, contentStoreCharacter.transform);
            item.GetComponent<NPCItem>().nameText.text = NPCManager.instance.npcs[i].name;
            item.GetComponent<NPCItem>().descriptionText.text = NPCManager.instance.npcs[i].description;

            item.GetComponent<NPCItem>().buyButton.onClick.AddListener(() => SummonCharacter(NPCManager.instance.npcs[i].namePrefab));

            items.Add(item.GetComponent<NPCItem>());

        }
    }

    public void SummonCharacter(string characterName)
    {
        //GameObject characterPrefab_ = charactersPrefabs.

        foreach (GameObject characterPrefab in charactersPrefabs)
        {
           // if (characterName == )
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
