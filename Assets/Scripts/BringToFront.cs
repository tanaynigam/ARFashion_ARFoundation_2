using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BringToFront : MonoBehaviour
{

    public float distance;
    public GameObject banner;
    public Text bannerText;
    public Camera camera;


    private bool isObjectInFront;
    private GameObject objectInFront;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    private bool lockTouchControls = false;

    private bool rotating = false;

    private int currentAudioSelection = 0;

    public Material[] audioMaterials;

    public string[] AudioText;

    public Text AudioTextField;

    public GameObject PlayButton;
    public GameObject PauseButton;
    // Start is called before the first frame update
    void Start()
    {
        isObjectInFront = false;
        currentAudioSelection = 0;
        VideoPlayer[] players = GetComponents<VideoPlayer>();
        foreach(VideoPlayer player in players)
        {
            player.SetDirectAudioMute(0, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        //if(Input.GetMouseButtonUp(0))
        {
            if (isObjectInFront && objectInFront.tag == "Model" && Input.GetTouch(0).phase == TouchPhase.Moved)
            //if (isObjectInFront && objectInFront.tag == "Model" && Input.mouseScrollDelta.magnitude > 0)
            {
                lockTouchControls = true;
                StopAllCoroutines();
                StartCoroutine(lockControlDelay());

                float xDelta = Input.GetTouch(0).deltaPosition.x;
                float yDelta = Input.GetTouch(0).deltaPosition.y;

                //float xDelta = Input.mouseScrollDelta.x;
                //float yDelta = Input.mouseScrollDelta.y;

                Quaternion original_rotation = objectInFront.transform.rotation;
                Quaternion temp_rotation = objectInFront.transform.rotation;

                objectInFront.transform.rotation *= Quaternion.AngleAxis(-xDelta / 3, Vector3.forward);
            }
            
            if (Input.GetTouch(0).phase == TouchPhase.Ended && !lockTouchControls)
            //if(Input.GetMouseButtonUp(0))
            {
                if (isObjectInFront)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Vector3 touchposition = touch1.position;
                    //Vector3 touchposition = Input.mousePosition;

                    RaycastHit hit1;
                    Ray ray1 = Camera.main.ScreenPointToRay(touchposition);
                    if (Physics.Raycast(ray1, out hit1, 2000))
                    {
                        if (hit1.transform.tag == "Controls")
                        {
                            AudioSource[] sources = objectInFront.transform.GetComponentsInChildren<AudioSource>();
                            if(hit1.transform.name == "PlayButton")
                            {
                                sources[currentAudioSelection].Play();
                                hit1.transform.gameObject.SetActive(false);
                                PauseButton.SetActive(true);
                            }
                            else if(hit1.transform.name == "PauseButton")
                            {
                                sources[currentAudioSelection].Pause();
                                hit1.transform.gameObject.SetActive(false);
                                PlayButton.SetActive(true);
                            }
                            else if(hit1.transform.name == "NextButton")
                            {
                                sources[currentAudioSelection].Stop();
                                currentAudioSelection++;
                                if (currentAudioSelection == sources.Length)
                                    currentAudioSelection = 0;

                                sources[currentAudioSelection].Play();
                                GameObject AudioObject = GameObject.Find("AudioObject");
                                AudioObject.GetComponent<MeshRenderer>().material.mainTexture = audioMaterials[currentAudioSelection].mainTexture;
                                AudioTextField.text = AudioText[currentAudioSelection];

                            }
                            else if(hit1.transform.name == "PreviousButton")
                            {
                                sources[currentAudioSelection].Stop();
                                currentAudioSelection--;

                                if (currentAudioSelection == -1)
                                    currentAudioSelection = sources.Length - 1;

                                sources[currentAudioSelection].Play();
                                GameObject AudioObject = GameObject.Find("AudioObject");
                                AudioObject.transform.GetComponent<MeshRenderer>().material.mainTexture = audioMaterials[currentAudioSelection].mainTexture;
                                AudioTextField.text = AudioText[currentAudioSelection];
                            }
                        }
                        else
                        {
                            objectInFront.transform.SetParent(originalParent);
                            objectInFront.transform.localPosition = originalPosition;
                            objectInFront.transform.localRotation = originalRotation;
                            isObjectInFront = false;
                            if (banner != null)
                                banner.SetActive(false);
                            if (objectInFront.transform.gameObject.GetComponentInChildren<VideoPlayer>())
                            {
                                objectInFront.transform.gameObject.GetComponentInChildren<VideoPlayer>().SetDirectAudioMute(0, true);
                            }

                            if (objectInFront.transform.gameObject.GetComponentInChildren<AudioSource>())
                            {
                                AudioSource[] sources = objectInFront.transform.gameObject.GetComponentsInChildren<AudioSource>();
                                sources[currentAudioSelection].Stop();
                                //objectInFront.transform.GetComponent<MeshRenderer>().material.mainTexture = audioMaterials[currentAudioSelection].mainTexture;
                                //AudioTextField.text = AudioText[currentAudioSelection];
                            }
                        }
                    }

                }
                else
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchposition = touch.position;
                    //Vector3 touchposition = Input.mousePosition;

                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touchposition);
                    if (Physics.Raycast(ray, out hit, 2000))
                    {
                        if (hit.transform.tag == "Frames")
                        {
                            originalPosition = hit.transform.localPosition;
                            originalRotation = hit.transform.localRotation;
                            originalParent = hit.transform.parent;


                            //hit.transform.localEulerAngles = new Vector3(0, 0, 0);
                            //hit.transform.Translate(camera.transform.position + camera.transform.forward * distance);

                            
                            hit.transform.position = camera.transform.position + camera.transform.forward * distance;
                            //hit.transform.LookAt(Camera.main.transform);
                            hit.transform.SetParent(camera.transform);
                            hit.transform.localEulerAngles = new Vector3(90, -180, 0);

                            hit.transform.parent = null;

                            objectInFront = hit.transform.gameObject;
                            if (hit.transform.gameObject.GetComponentInChildren<VideoPlayer>())
                            {
                                hit.transform.gameObject.GetComponentInChildren<VideoPlayer>().SetDirectAudioMute(0, false);
                                hit.transform.gameObject.GetComponentInChildren<VideoPlayer>().Stop();
                                hit.transform.gameObject.GetComponentInChildren<VideoPlayer>().Play();
                            }   

                            if (hit.transform.gameObject.GetComponentInChildren<AudioSource>())
                            {
                                AudioSource[] sources = hit.transform.gameObject.GetComponentsInChildren<AudioSource>();
                                currentAudioSelection = 0;
                                sources[currentAudioSelection].Stop();
                                sources[currentAudioSelection].Play(); 
                                GameObject AudioObject = GameObject.Find("AudioObject");
                                AudioObject.GetComponent<MeshRenderer>().material.mainTexture = audioMaterials[currentAudioSelection].mainTexture;
                                AudioTextField.text = AudioText[currentAudioSelection];
                            }

                            foreach(Transform t in hit.transform)
                            {
                                if (t.name == "Banner")
                                    banner = t.gameObject;
                            }
                            if(banner != null)
                                banner.SetActive(true);
                            //bannerText.text = hit.transform.gameObject.name;
                        }
                        else if (hit.transform.tag == "Model")
                        {
                            originalPosition = hit.transform.localPosition;
                            originalRotation = hit.transform.localRotation;
                            originalParent = hit.transform.parent;
                            hit.transform.parent = null;
                            hit.transform.position = camera.transform.position + camera.transform.forward * distance;
                            hit.transform.SetParent(camera.transform);
                            hit.transform.localEulerAngles = new Vector3(90, 180, 0);

                            hit.transform.parent = null;
                            //hit.transform.LookAt(Camera.main.transform, hit.transform.forward);
                            //hit.transform.localEulerAngles = new Vector3(-camera.transform.localEulerAngles.x + 90, -camera.transform.localEulerAngles.y + 180, -camera.transform.localEulerAngles.z);
                            objectInFront = hit.transform.gameObject;
                        }
                    }
                    isObjectInFront = true;
                }
            }
        }

    }


    IEnumerator lockControlDelay()
    {
        yield return new WaitForSeconds(0.5f);
        lockTouchControls = false;
    }
}
