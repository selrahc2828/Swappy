using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent] // empeche le componenet d'etre ajoute plusieurs fois a l'objet
public class UniqueID : MonoBehaviour
{
    #if UNITY_EDITOR

    [SerializeField]
    private string uniqueId;
    public string UniqueId => uniqueId;
    
    private void Awake()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            CheckUniqueID();
        }
        #endif
    }
    
    private void OnValidate()
    {
        #if UNITY_EDITOR

        if (!Application.isPlaying)
        {
            CheckUniqueID();
        }
        #endif
    }

    
    public void CheckUniqueID()
    {
        if (string.IsNullOrEmpty(uniqueId) || !UniqueIDManagerEditor.RegisterID(uniqueId, this))
        {
            uniqueId = UniqueIDManagerEditor.GenerateNewID(this);
            
            // Mettre à jour SpawnPot si présent
            SpawnPot spawner = GetComponent<SpawnPot>();
            if (spawner != null)
            {
                spawner.UniqueID = uniqueId;
            }
        }
        else
        {
            UniqueIDManagerEditor.RegisterID(uniqueId, this); // Redondant mais sûr
        }
    }
    #endif
}
