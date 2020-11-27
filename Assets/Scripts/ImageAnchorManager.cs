using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ImageAnchorManager : MonoBehaviour
{
    public GameObject Experience;
    public float distance;
    private Vector3 imagePosition;

    public GameObject MiddlePanel;

    private bool tracking = true;
    // Start is called before the first frame update
    void Start()
    {
        MiddlePanel.SetActive(true);
        imagePosition = new Vector3(0, 0, 0);
        tracking = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject anchor = GameObject.FindWithTag("Anchor");
        if(anchor != null)
            imagePosition = anchor.transform.position;

        if (anchor != null && tracking) // && Vector3.Distance(Camera.main.transform.position, imagePosition) < distance)
        {

            VideoPlayer[] players = GetComponents<VideoPlayer>();
            foreach (VideoPlayer player in players)
            {
                player.SetDirectAudioMute(0, true);
            }

            MiddlePanel.SetActive(false);

            StartCoroutine(ToggleTracking());
            //imagePosition = anchor.transform.position;
            Experience.SetActive(true);
            Experience.transform.position = anchor.transform.position;
            Experience.transform.rotation = anchor.transform.rotation;
            //Experience.transform.localScale = anchor.transform.localScale;
            imagePosition = Experience.transform.position;
            Experience.transform.eulerAngles = new Vector3(-90, Experience.transform.eulerAngles.y, Experience.transform.eulerAngles.z);
        }

        //if(Vector3.Distance(Camera.main.transform.position, imagePosition) > distance)
        //{
        //    Experience.SetActive(false);
        //}
    }

    IEnumerator ToggleTracking()
    {
        yield return new WaitForSeconds(1f);
        tracking = false;
    }
}
