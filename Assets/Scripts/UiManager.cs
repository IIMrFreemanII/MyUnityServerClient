using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
     public static UiManager Instance;
     public GameObject startMenu;
     public TMP_InputField usernameField;

     private void Awake()
     {
          if (Instance == null)
          {
               Instance = this;
          } else if (Instance != this)
          {
               Debug.Log("Instance already exists, destroying object!");
          }
     }

     public void ConnectToServer()
     {
          NetworkManager.ConnectToServer("127.0.0.1", 8000);
          // startMenu.SetActive(false);
          // usernameField.interactable = false;
     }

     [ContextMenu("Connect with name")]
     public void SendMyName()
     {
          string name = usernameField.text;

          if (name == "")
          {
               Debug.Log("Enter your name");
               return;
          }
          ClientTcp.clientName = name;
          
          DataSender.SentClientName(name);
     }
}
