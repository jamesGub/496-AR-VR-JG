using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; 
using TMPro; 
using UnityEngine.EventSystems;

public class RaycastControl : MonoBehaviour
{
    public GameObject spawnItem; 
    public TextMeshProUGUI scoreFullText;
    public TextMeshProUGUI loveFullText; 
    public float scoreFullRate = 0.3f; 
    public float scoreLoveRate = 0.4f; 
    ARRaycastManager ar_Manager; 
    Camera cam;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    RaycastHit p_Hit; 
    GameObject spawnedObject; 
    float scoreFull;
    float scoreLove; 
    char state = 'i';
    private float timeInState = 0; 

    void Start()
    {
        ar_Manager = GetComponent<ARRaycastManager>(); 
        cam = GetComponentInChildren<Camera>(); 
        scoreFull = 0; 
        scoreLove = 0; 
         
    }

    void Update()
    {
        scoreFull -= Time.deltaTime * scoreFullRate;
        scoreLove -= Time.deltaTime * scoreLoveRate; 

        if (state == 'f' || state == 'p') { 
            
            timeInState += Time.deltaTime; 
            if (timeInState >= 5) { 
                state = 'i'; 

            }
        }

        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) { 
            
            if (EventSystem.current.IsPointerOverGameObject()) {
                print("UI Touch Detected"); 
                return; 
            }
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
                    //if in feed state
                    if (state == 'f') { 
                    scoreFull += 1;
                    scoreFull.Mathf.Clamp(scoreLove, 0, 100); 
                    scoreFullText.text = "This is your score!!! :D: " + Mathf.Round(scoreFull).ToString();  
                    } else if (state == 'p') { 
                        scoreLove += 1; 
                        scoreLove.Mathf.Clamp(scoreLove, 0, 100); 
                        loveFullText.text = "Love: " + Mathf.Round(scoreLove).ToString(); 
                        
                    }
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

    public void EnterFeedState() { 
        state = 'f'; 
        timeInState = 0;
    }

    public void EnterPetState() { 
        state = 'p';
        timeInState = 0; 
    }
}
