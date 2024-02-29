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
    Animator KittyAnim; 
    public float scoreFullRate = 0.3f; 
    public float scoreLoveRate = 0.4f; 
    public float timeToSpendInState = 5;
    ARRaycastManager ar_Manager; 
    Camera cam;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    RaycastHit p_Hit; 
    GameObject spawnedObject; 
    private float scoreFull;
    private float scoreLove; 
    char state = 'i';
    private float timeInState = 0; 

    void Start()
    {
        ar_Manager = GetComponent<ARRaycastManager>(); 
        cam = GetComponentInChildren<Camera>(); 
        scoreFull = 100; 
        scoreLove = 100; 
         
    }

    void Update()
    {
        if (state == 'i') { 
            scoreFull -= Time.deltaTime * scoreFullRate;
            scoreLove -= Time.deltaTime * scoreLoveRate;
        }
         

        if (state == 'f' || state == 'p') { 
            
            timeInState += Time.deltaTime; 
            if (timeInState >= timeToSpendInState) { 
                state = 'i'; 
                KittyAnim.SetTrigger("EnterIdle"); 

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
                   
                    } else if (state == 'p') { 
                        scoreLove += 1; 
                       
                        
                    }
                }
           
            }
            
            //AR Ray
          
            if (ar_Manager.Raycast(r, hits)) 
            { 
                Pose hitPose = hits[0].pose; 
                if (spawnedObject == null) { 
                spawnedObject = Instantiate(spawnItem, hitPose.position, hitPose.rotation);
                KittyAnim = spawnedObject.GetComponentInChildren<Animator>(); 
                } else { 
                    spawnedObject.transform.position = hitPose.position;  
                }
                Vector3 lookAtTarget = new Vector3(cam.transform.position.x, spawnedObject.transform.position.y, cam.transform.position.z);
                spawnedObject.transform.LookAt(lookAtTarget);

            }
        }
        
        scoreFull = Mathf.Clamp(scoreLove, 0, 100); 
        scoreLove = Mathf.Clamp(scoreLove, 0, 100); 


    }

    void SetFullScoreText() { 
        scoreFullText.text = "This is your score!!! :D: " + Mathf.Round(scoreFull).ToString();
    }

    void SetPetScoreText() { 
        loveFullText.text = "Love: " + Mathf.Round(scoreLove).ToString(); 

    }

    public void EnterFeedState() { 
        state = 'f'; 
        timeInState = 0;
        KittyAnim.SetTrigger("EnterFeed"); 
    }

    public void EnterPetState() { 
        state = 'p';
        timeInState = 0; 
        KittyAnim.SetTrigger("EnterPet");
    }
}
