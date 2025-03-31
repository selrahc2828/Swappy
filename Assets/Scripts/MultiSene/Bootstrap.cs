using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.LoadScene("SC_Main_Menu", additive: true);
    }
}