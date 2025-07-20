using UnityEngine;

public class MoreSpeedProject : MonoBehaviour
{
    public float speedAdd;
    public IProjectBasic iProject;
    void Start() {
        if (iProject == null) iProject = GetComponent<IProjectBasic>();
        iProject.AddSpeed(speedAdd);
    }
}
