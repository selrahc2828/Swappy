using System;
using System.IO; // travaille avec fichier et dossier, accès en lecture et ecriture
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameSaveData saveData;
    
    public static SaveManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);//détuit le doublon
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }
    
    public string SaveFileName()
    {
        // persistentDataPath = chemin dossier avec droit d'acces en ecriture
        string dir = Application.persistentDataPath + "/Saves";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        
        return dir + "/SaveData.json";
    }

    public void SaveFileGame()
    {
        string jsonFile = JsonUtility.ToJson(saveData, true); // conversion d'une classe au format JSON, true => on ne compresse pas en ligne
        // jsonFile == donnee de la sauvegarde
        string filePath = SaveFileName();
        File.WriteAllText(filePath, jsonFile);
        //File = acces a un fichier
        //WriteAllText = ecriture de texte, parametre (chemin, donnees)
    }

    public void LoadFileGame()
    {
        if (File.Exists(SaveFileName()))
        {
            try
            {
                string jsonFile = File.ReadAllText(SaveFileName());
                saveData = JsonUtility.FromJson<GameSaveData>(jsonFile);
            }
            catch (Exception e)
            {
                Debug.LogError($"Erreur lors du chargement du fichier : {e.Message}");
            }
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
        LoadAll();
    }

    void OnApplicationQuit()
    {
        SaveAll();
    }

    public void SaveAll()
    {
        Debug.Log($"Save Datas");
        SaveData();
        SaveFileGame();
    }

    private void LoadAll()
    {
        LoadSpawner();
        LoadFragment();
        
        // PB : position bien récupéré et set (testé sur un cube) mais doit être override ailleurs
        // GameObject player = GameManager.Instance.player;
        // player.transform.position = saveData.player.playerPosition;
        // player.transform.rotation = saveData.player.playerRotation;
    }
    
    private void SaveSpawnerPotData()
    {
        SpawnPot[] spawners = FindObjectsOfType<SpawnPot>(true);
        
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
        SpawnPot[] spawners = FindObjectsOfType<SpawnPot>(true);
        Debug.Log($"load SPAWNERPOT : {spawners.Length}");

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
