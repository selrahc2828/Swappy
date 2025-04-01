using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableScene/SceneData", order = 0)]
public class SceneData : ScriptableObject
{
    public bool isOpen;
    public bool isPersistent;
}
