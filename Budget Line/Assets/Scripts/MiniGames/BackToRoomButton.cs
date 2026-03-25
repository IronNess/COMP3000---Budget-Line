using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToRoomButton : MonoBehaviour
{
    public void ReturnToRoom()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("RoomScene");
    }
}