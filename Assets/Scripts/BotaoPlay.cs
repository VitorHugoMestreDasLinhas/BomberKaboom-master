
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoPlay : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
