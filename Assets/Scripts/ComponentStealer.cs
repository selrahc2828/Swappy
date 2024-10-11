using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Reflection;
using UnityEngine.SceneManagement;

public class ComponentStealer : MonoBehaviour
{
    public TextMeshProUGUI surface;
    public TextMeshProUGUI lastSteal;

    [Header("Raycast et line render")]
    private RaycastHit _hit;
    [HideInInspector]
    public float radius;
    public LineRenderer line;
    public float longueur = 10f;

    //public bool isStealing;
    public List<Component> listComportement = new List<Component>();
    public Camera mainCam;
    public Transform rayPointStrat;
    public Transform castStealerPoint;

    [Header("Variation")]
    [Tooltip("Vol ou copie")]
    public bool isSteal;

    private void OnEnable()
    {

    }

    void Start()
    {
        lastSteal.text = "";
    }

    void Update()
    {
        RaycastHit _hit;
        Vector3 rayTail = mainCam.transform.position + mainCam.transform.forward * longueur;
        Ray _ray = new Ray(mainCam.transform.position, rayTail);
        line.SetPosition(0, rayPointStrat.position);
        line.SetPosition(1, rayTail);

        //if (Physics.SphereCast(_ray, radius, out _hit, Mathf.Infinity)) //mask

        if (Physics.Raycast(castStealerPoint.position, castStealerPoint.forward, out _hit, Mathf.Infinity)) //mask
        {
            if (_hit.collider == null)
            {
                surface.SetText("null");
                return;
            }
            //Debug.Log(_hit.collider.gameObject.name);
            //nameText.text = _hit.collider.gameObject.name;
            surface.SetText(_hit.collider.gameObject.name);
        }
    }

    public void CopyStealComp()
    {
        //Debug.Log("StealComp  _mvtData.type : " + _mvtData.type);
        //isStealing = true;
        if (Physics.Raycast(castStealerPoint.position, castStealerPoint.forward, out _hit, Mathf.Infinity)) //mask
        {
            if (_hit.collider == null)
            {
                return;
            }

            if (_hit.collider.gameObject.GetComponent<Comportment>())
            {
                //Debug.Log(_hit.collider.gameObject.GetComponent<Comportement>().typeComp);

                Comportment[] listComp = _hit.collider.gameObject.GetComponents<Comportment>();

                foreach (Comportment item in listComp)
                {
                    listComportement.Add(item);

                    lastSteal.text += item.name + " - " + item.typeComp + "\n";

                    if (isSteal)
                        Destroy(item);// ou disable
                }
            }

        }
    }

    public void PasteComp()
    {
        //Debug.Log("PasteComp  _mvtData.type : " + _mvtData.type);
        //isStealing = false;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out _hit, Mathf.Infinity)) //mask
        {
            if (_hit.collider == null || _hit.collider.GetComponent<Rigidbody>() == null)
            {
                return;
            }

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
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(mainCam.transform.position, mainCam.transform.position + mainCam.transform.forward * 10);
    }
}
