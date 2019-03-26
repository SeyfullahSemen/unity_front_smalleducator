using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CreateLessonScript : MonoBehaviour
{

    public Button goBackButton;
    public Button createButton;
    public InputField lesNaam;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /**
     * Init function to declare the gameobjects
     */
    private void init() {

        goBackButton = GameObject.FindGameObjectWithTag("goBackButton").GetComponent<Button>();
        createButton = GameObject.FindGameObjectWithTag("voegLesToeButton").GetComponent<Button>();
        lesNaam = GameObject.FindGameObjectWithTag("lesNaamField").GetComponent<InputField>();

    }

  
    public void goBackOnClick() {
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }

    public void createLessonOnClick() {
        string newLesson = lesNaam.text.ToString(); 
        if (newLesson == "" || newLesson.Length == 0) {
            Debug.Log("Please enter a valid lesson name into the input field!");
        }
        else{
            StartCoroutine(postNewLesson(newLesson)); // function to create a new lesson
            Debug.Log("Creating new lesson " + newLesson);
        }
        
    }


    IEnumerator postNewLesson(string newLesson) {
       var uwr = new UnityWebRequest("http://localhost:8080/lesson", "POST"); // connect to the backend
        
        string jsonToSend1 = "{\"lessonname\":\"" + newLesson + "\"}";
        
       byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonToSend1);
       uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
       uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
       uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        // Check if there are network errors
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

    }

}

