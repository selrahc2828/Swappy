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
    }
    
    void OnApplicationQuit()
    {
        SaveData();
        SaveFileGame();
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
        }    }

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

        int hasCollectCurrent = saveData.fragment.hasCollect;
        
        foreach (FragmentObject frag in FindObjectsOfType<FragmentObject>())
        {
            hasCollectCurrent += frag.Quantity;
        }
        
        saveData.fragment.hasCollect = hasCollectCurrent;
    }

    private void LoadFragment()
    {
        Debug.LogWarning($"loadFragment : {saveData.fragment.fragmentBank} : {saveData.fragment.hasCollect}");
        FragmentSystem.Instance.SetFragment(saveData.fragment.fragmentBank);
        FragmentSystem.Instance.SetFragmentHasCollect(saveData.fragment.hasCollect);
        Debug.LogWarning($"scriptable  : {FragmentSystem.Instance.fragmentBankData.bankInventoryFragmentQuantity} : {FragmentSystem.Instance.fragmentBankData.hasCollect}");

    }
}
