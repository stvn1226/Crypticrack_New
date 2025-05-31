using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTools : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
