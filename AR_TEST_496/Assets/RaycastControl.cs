using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; 
using TMPro; 

public class RaycastControl : MonoBehaviour
{
    public GameObject spawnItem; 
    public TextMeshProUGUI scoreText; 
    ARRaycastManager ar_Manager; 
    Camera cam;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    RaycastHit p_Hit; 
    GameObject spawnedObject; 
    int score;

    void Start()
    {
        ar_Manager = GetComponent<ARRaycastManager>(); 
        cam = GetComponentInChildren<Camera>(); 
        score = 0; 
         
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) { 
            Vector3 position; 
            if (Input.touchCount > 0) { 
                position = Input.GetTouch(0).position; 
            } else { 
                position = Input.mousePosition; 
            }
            
            Ray r = cam.ScreenPointToRay(position); 

            //Physics Ray
            if (Physics.Raycast(r,out p_Hit)) { 
                //Score
                if(p_Hit.transform.gameObject.CompareTag("Sphere")) { 
                    Debug.Log("hit");
                    score += 1;
                    scoreText.text = "This is your score!!! :D: " + score.ToString();  
                }
           
            }
            
            //AR Ray
          
        if (ar_Manager.Raycast(r, hits)) 
            { 
                Pose hitPose = hits[0].pose; 
                if (spawnedObject == null) { 
                spawnedObject = Instantiate(spawnItem, hitPose.position, hitPose.rotation);
                } else { 
                    spawnedObject.transform.position = hitPose.position;  
                }

            }
        }
    }
}
