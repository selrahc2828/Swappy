using System.IO; // travaille avec fichier et dossier, accès en lecture et ecriture
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameSaveData saveData;
    public GameObject test;
    public string SaveFileName()
    {
        // persistentDataPath = chemin dossier avec droit d'acces en ecriture
        string dir = Application.persistentDataPath + "/Saves";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        
        return dir + "/SaveData.txt";;
    }

    public void SaveFileGame()
    {
        string jsonFile = JsonUtility.ToJson(saveData); // conversion d'une classe au format JSON
        // jsonFile == donnee de la sauvegarde
        File.WriteAllText(SaveFileName(), jsonFile);
        //File = acces a un fichier
        //WriteAllText = ecriture de texte, parametre (chemin, donnees)
    }

    public void LoadFileGame()
    {
        if (File.Exists(SaveFileName()))
        {
            string jsonFile = File.ReadAllText(SaveFileName());
            saveData = JsonUtility.FromJson<GameSaveData>(jsonFile);
        }
        else
        {
            Debug.Log("Le fichier de sauvegarde n'existe pas");
        }
    }

    void SaveData()
    {
        SaveSpawnerPotData();
        SaveFragment();

        // GameObject player = GameManager.Instance.player;
        // saveData.player.playerPosition = player.transform.position;
        // saveData.player.playerRotation = player.transform.rotation;
    }


    private void Start()
    {
        LoadFileGame();

        LoadSpawner();
        LoadFragment();

        // PB : position bien récupéré et set (testé sur un cube) mais doit être overide ailleurs
        // GameObject player = GameManager.Instance.player;
        // player.transform.position = saveData.player.playerPosition;
        // player.transform.rotation = saveData.player.playerRotation;

        if (test)
        {
            test.transform.position = saveData.player.playerPosition;
        }
        else
        {
            Debug.Log($"Tu as oublié de mettre l'objet test :)");
        }
    }

    void OnDisable()
    {
        SaveData();
        SaveFileGame();
        Debug.LogWarning("dans OnDisable");
    }
    
    void OnApplicationQuit()
    {
        // SaveData();
        // SaveFileGame();
        // Debug.LogWarning("dans OnApplicationQuit");

    }

    public void SaveOnce()
    {
        //evite de faire la save dans OnDisable et OnApplicationQuit
    }
    
    private void SaveSpawnerPotData()
    {
        SpawnPot[] spawners = FindObjectsOfType<SpawnPot>();

        foreach (SpawnPot spawner in spawners)
        {
            // Chercher s'il existe déjà dans la sauvegarde
            var existing = saveData.spawnerPots.Find(p => p.id == spawner.UniqueID);

            if (existing is not null) // met a jour
            {
                existing.isBroken = spawner.isBroken;
            }
            else // creer un nouveau
            {
                saveData.spawnerPots.Add(new SpawnPotSaveData
                    {
                        id = spawner.UniqueID,
                        isBroken = spawner.isBroken,
                    }
                );
            }
        }
    }

    private void LoadSpawner()
    {
        SpawnPot[] spawners = FindObjectsOfType<SpawnPot>();

        foreach (SpawnPot spawner in spawners)
        {
            // Chercher s'il existe déjà dans la sauvegarde
            SpawnPotSaveData existing = saveData.spawnerPots.Find(p => p.id == spawner.UniqueID);

            if (existing is not null) // met a jour
            {
                spawner.isBroken = existing.isBroken;
            }
        }
    }

    private void SaveFragment()
    {
        saveData.fragment.fragmentBank = FragmentSystem.Instance.fragmentBankData.bankInventoryFragmentQuantity;
        saveData.fragment.hasCollect = FragmentSystem.Instance.fragmentBankData.hasCollect; 
        
        int hasCollectCurrent = saveData.fragment.hasCollect;// valeur initiale
        
        foreach (FragmentObject frag in FindObjectsOfType<FragmentObject>())//puis on ajoute ce qu'il y a dans la scene
        {
            hasCollectCurrent += frag.Quantity;
        }

        saveData.fragment.hasCollect = hasCollectCurrent;
        FragmentSystem.Instance.fragmentBankData.hasCollect = hasCollectCurrent;
    }

    private void LoadFragment()
    {
        FragmentSystem.Instance.SetFragment(saveData.fragment.fragmentBank);
        FragmentSystem.Instance.SetFragmentHasCollect(saveData.fragment.hasCollect);
    }
}
