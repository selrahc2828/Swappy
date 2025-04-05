using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public SceneReference mainscene;
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.LoadScene(mainscene.BuildIndex);
    }
}