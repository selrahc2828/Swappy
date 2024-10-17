using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.HID;

public enum TypeCopy
{
    Copie1DonneEtPerd_DonneMoi,
    CopieMultipleEtConserve
}

public class ComponentStealer : MonoBehaviour
{
    public TextMeshProUGUI surface;
    public TextMeshProUGUI lastSteal;

    private Vector3 rayTail;
    private Ray _ray;

    public Controller controller;
    public CameraController camController;

    [Header("Raycast et line render")]
    private RaycastHit _hit;
    [HideInInspector]
    public float radius;
    public LineRenderer line;
    public float longueur = 10f;
    public LayerMask hitLayer;

    //public bool isStealing;
    public List<Component> listComportement = new List<Component>();
    private Camera mainCam;
    public Transform rayPointStrat;
    public Transform castStealerPoint;//à suppr

    [Header("Variations")]
    [Tooltip("Copie1DonneEtPerd = Celui de Charles, 1 seul copie possible et on la perd quand on la donne \n " +
        "CopieMultipleEtConserve = Celui de Greg, possible de voler plusieurs et de tout donner en 1 fois, on les garde après")]
    public TypeCopy typeCopy;
    [Tooltip("Vol ou copie")]
    public bool isSteal;
    public bool copyPasteSpawner = false;
    [Header("Pour CopieMultipleEtConserve")]
    public int maxSteal;
    public int nbSteal;
    public MonoBehaviour[] components;

    [Header("Pour Copie1DonneEtPerd_DonneMoi")]

    public GameObject objectStolen;
    public Dictionary<MonoBehaviour, System.Type> steals;
    public System.Type type;

    private void OnEnable()
    {

    }

    void Start()
    {
        lastSteal.text = "";
        mainCam = camController.mainCamera;
        steals = new Dictionary<MonoBehaviour, System.Type>();

    }

    void Update()
    {
        RaycastHit _hit;

        mainCam = camController.Brain.OutputCamera;

        //Vector3 newMousePos = new Vector3(Mathf.Abs(Input.mousePosition.x), Mathf.Abs(Input.mousePosition.y), Mathf.Abs(Input.mousePosition.z));
        _ray = mainCam.ScreenPointToRay(Input.mousePosition);

        //rayTail = mainCam.transform.position + mainCam.transform.forward * longueur;

        float maxDistance = 500f;
        // Si le raycast touche un objet
        if (Physics.Raycast(_ray, out _hit, maxDistance, hitLayer))
        {
            // Positionner les points du LineRenderer pour dessiner la ligne
            line.SetPosition(0, rayPointStrat.position);  // Début de la ligne (caméra)
            line.SetPosition(1, _hit.point);  // Fin de la ligne (point touché par le rayon)

            Debug.DrawLine(mainCam.transform.position, _hit.point, Color.red);
        }
        else
        {
            // Si rien n'est touché, on dessine la ligne jusqu'à la distance max du raycast
            Vector3 farPoint = _ray.GetPoint(maxDistance);
            line.SetPosition(0, rayPointStrat.position);  // Début de la ligne (caméra)
            line.SetPosition(1, farPoint);  // Fin de la ligne (point éloigné)
            Debug.DrawLine(mainCam.transform.position, farPoint, Color.green);

        }

        //if (Physics.SphereCast(_ray, radius, out _hit, Mathf.Infinity)) //mask

        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer))// (Physics.Raycast(castStealerPoint.position, castStealerPoint.forward, out _hit, Mathf.Infinity, hitLayer))
        {
            if (_hit.collider == null)
            {
                surface.SetText("null");
                return;
            }
            surface.SetText(_hit.collider.gameObject.name);
        }
        else
        {
            surface.SetText("");
        }
    }

    public void CopyStealComp()
    {
        //Debug.Log("StealComp  _mvtData.type : " + _mvtData.type);
        //isStealing = true;
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            if (copyPasteSpawner)
            {
                if (_hit.transform.gameObject.CompareTag("Spawner"))
                {
                    objectStolen = _hit.collider.gameObject;
                    ModifiedSpawner SpawnerScript = objectStolen.GetComponent<ModifiedSpawner>();
                    steals = null;
                    steals = SpawnerScript.StealsToApply;

                    objectStolen = null;
                    SpawnerScript.StealsToApply = null;

                    Debug.Log("Got from Spawner these Steals: " + steals);
                    return;
                }
            }


            if (_hit.collider == null)
            {
                return;
            }


            if (_hit.collider.gameObject.GetComponent<Comportment>())
            {
                switch (typeCopy)
                {
                    case TypeCopy.Copie1DonneEtPerd_DonneMoi:
                        objectStolen = _hit.collider.gameObject;
                        components = objectStolen.GetComponents<MonoBehaviour>();
                        #region pour voler un objet a la fois
                        if (steals != null)
                        {
                            //on parcour le dictionnaire
                            foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
                            {
                                //si le monobehavior existe encore (verification obligatoire en cas de destruction de objectStolen)
                                if (script.Key != null)
                                {
                                    //on le réactive
                                    script.Key.enabled = true;
                                }
                                else
                                {
                                    //on arrete de parcourir le dictionnaire car il contiens des valeurs appartenant a un objet détruit
                                    break;
                                }
                            }
                            //on clear le dictionnaire
                            steals.Clear();
                            lastSteal.text = "";

                        }
                        #endregion

                        foreach (MonoBehaviour component in components)
                        {
                            //on sauvegarde son type (le nom du script qu'on va voler s'y trouve)
                            type = component.GetType();
                            //on sauvgarde les field (les valeurs des variables du script qu'on va voler)
                            FieldInfo stealableField = type.GetField("stealable");

                            //s'il y a une valeurs dans stealableField et que cette valeur est un booléen
                            if (stealableField != null && stealableField.FieldType == typeof(bool))
                            {
                                //on copie le booléen stealable du script volé
                                bool isStealable = (bool)stealableField.GetValue(component);

                                //on verifie qu'il est true
                                if (isStealable)
                                {
                                    //on ajoute une entrée au dictionnaire avec en clé le component et en value son type
                                    steals.Add(component, type);
                                    // Désactive le script en le mettant inactif
                                    component.enabled = false;
                                    lastSteal.text += component.name + " - " + type + "\n";
                                }
                            }
                        }
                        break;
                    case TypeCopy.CopieMultipleEtConserve:
                        //legacy code
                        Comportment[] listComp = _hit.collider.gameObject.GetComponents<Comportment>();

                        foreach (Comportment item in listComp)
                        {
                            if (nbSteal >= maxSteal)
                            {
                                listComportement.RemoveAt(0);
                                string[] lines = lastSteal.text.Split('\n');

                                // Vérifier qu'il y a plus d'une ligne pour pouvoir supprimer
                                if (lines.Length > 1)
                                {
                                    // Recréer le texte sans la première ligne
                                    lastSteal.text = string.Join("\n", lines, 1, lines.Length - 1);
                                }
                            }

                            listComportement.Add(item);
                            lastSteal.text += item.name + " - " + item.typeComp + "\n";

                            if (isSteal)
                                Destroy(item);// ou disable

                            nbSteal++;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void PasteComp()
    {
        //Debug.Log("PasteComp  _mvtData.type : " + _mvtData.type);
        //isStealing = false;
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            //Debug.LogWarning("Hit ray donné : " + _hit.collider.name);

            if (copyPasteSpawner)
            {
                if (_hit.transform.gameObject.tag == "Spawner")
                {
                    GameObject objectGiven = _hit.collider.gameObject;
                    ModifiedSpawner SpawnerScript = objectGiven.GetComponent<ModifiedSpawner>();

                    if (steals != null)
                    {
                        foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
                        {
                            SpawnerScript.StealsToApply.TryAdd(script.Key, script.Value);
                        }

                    }

                    return;
                }
            }


            if (_hit.collider == null || _hit.collider.GetComponent<Rigidbody>() == null)
            {
                return;
            }

            switch (typeCopy)
            {
                case TypeCopy.Copie1DonneEtPerd_DonneMoi:
                    //si l'objectStolen existe (on a bien un script volé)
                    if (objectStolen != null && objectStolen.GetComponent<Collider>() != null)
                    {
                        GameObject objectGiven = _hit.collider.gameObject;

                        //on parcour le dictionnaire des scripts a appliquer
                        foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
                        {
                            // Ajoute dynamiquement un script du même type sur l'objet cible
                            Component newComponent = objectGiven.AddComponent(script.Value);

                            //on parcour les variable du script qu'on vien d'ajouter
                            foreach (FieldInfo field in script.Value.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                            {
                                //on leur donne les même valeur qu'au moment ou on a volé le script
                                field.SetValue(newComponent, field.GetValue(script.Key));
                            }
                            //on détruit le script dans l'objet d'origine
                            Destroy(script.Key);
                            Debug.Log("Script " + script.Value.Name + " copié sur " + objectGiven.name + " et supprimé de " + objectStolen.name);
                        }
                        //on remet a zero les dictionnaire et les objets sauvegardé
                        steals.Clear();
                        lastSteal.text = "";
                        components = null;
                        objectStolen = null;
                    }
                    break;
                case TypeCopy.CopieMultipleEtConserve:
                    // legacy code
                    // Désactiver chaque script
                    foreach (Comportment component in listComportement)
                    {
                        System.Type type = component.GetType();
                        //FieldInfo stealableField = type.GetField("stealable");//nope

                        // Ajoute dynamiquement un script du même type sur l'objet cible
                        Component newComp = _hit.collider.gameObject.AddComponent(type);


                        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                        {
                            field.SetValue(newComp, field.GetValue(component));
                        }
                    }
                    break;
                default:
                    break;
            }

        }
    }

    public void PasteAtMe()
    {
        //si l'objectStolen existe (on a bien un script volé)
        if (objectStolen != null && objectStolen.GetComponent<Collider>() != null)
        {
            GameObject objectGiven = gameObject; //à modif plus tard

            //on parcour le dictionnaire des scripts a appliquer
            foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
            {
                // Ajoute dynamiquement un script du même type sur l'objet cible
                Component newComponent = objectGiven.AddComponent(script.Value);

                //on parcour les variable du script qu'on vien d'ajouter
                foreach (FieldInfo field in script.Value.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    //on leur donne les même valeur qu'au moment ou on a volé le script
                    field.SetValue(newComponent, field.GetValue(script.Key));
                }
                //on détruit le script dans l'objet d'origine
                Destroy(script.Key);
                Debug.Log("Script " + script.Value.Name + " copié sur " + objectGiven.name + " et supprimé de " + objectStolen.name);
            }
            //on remet a zero les dictionnaire et les objets sauvegardé
            steals.Clear();
            lastSteal.text = "";
            components = null;
            objectStolen = null;
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(mainCam.transform.position, mainCam.transform.position + mainCam.transform.forward * 10);
    }
}
