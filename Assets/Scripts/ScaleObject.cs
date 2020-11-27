using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Model;
    public float scaleSensitivity;
    public float positionSensitivity;
    public float rotationSensitivity;

    public Text DebugPanel;

    private string xScale;
    private string yScale;
    private string zScale;

    private string xPosition;
    private string yPosition;
    private string zPosition;

    private string xRot;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xScale = "X : " + Model.transform.localScale.x;
        yScale = "Y : " + Model.transform.localScale.y;
        zScale= "Z : " + Model.transform.localScale.z;

        xPosition = "X : " + Model.transform.localPosition.x;
        yPosition = "Y : " + Model.transform.localPosition.y;
        zPosition = "Z : " + Model.transform.localPosition.z;

        xRot = "Degree: " + Model.transform.localEulerAngles.x;

        DebugPanel.text = "SCALE: \n" + xScale + "\n" + yScale + "\n" + zScale + "\n\n" + "Position: \n" + xPosition + "\n" + yPosition + "\n" + zPosition + "\n\n" + "Rotation: " + xRot;
    }


    #region POSITION
    public void HandleUpClick()
    {
        Model.transform.localPosition = new Vector3(Model.transform.localPosition.x, Model.transform.localPosition.y, Model.transform.localPosition.z + positionSensitivity);
    }

    public void HandleDownClick()
    {
        Model.transform.localPosition = new Vector3(Model.transform.localPosition.x, Model.transform.localPosition.y, Model.transform.localPosition.z - positionSensitivity);
    }

    public void HandleLeftClick()
    {
        Model.transform.localPosition = new Vector3(Model.transform.localPosition.x + positionSensitivity, Model.transform.localPosition.y, Model.transform.localPosition.z);
    }

    public void HandleRightClick()
    {
        Model.transform.localPosition = new Vector3(Model.transform.localPosition.x - positionSensitivity, Model.transform.localPosition.y, Model.transform.localPosition.z);
    }

    public void HandlePositiveZClick()
    {
        Model.transform.localPosition = new Vector3(Model.transform.localPosition.x, Model.transform.localPosition.y + positionSensitivity, Model.transform.localPosition.z);
    }

    public void HandleNegativeZClick()
    {
        Model.transform.localPosition = new Vector3(Model.transform.localPosition.x, Model.transform.localPosition.y - positionSensitivity, Model.transform.localPosition.z);
    }

    #endregion

    #region SCALE
    public void HandlePositiveScaleClick()
    {
        Model.transform.localScale = Model.transform.localScale + new Vector3(scaleSensitivity, scaleSensitivity, scaleSensitivity);
    }

    public void HandleNegativeScaleClick()
    {
        Model.transform.localScale = Model.transform.localScale - new Vector3(scaleSensitivity, scaleSensitivity, scaleSensitivity);
    }
    #endregion

    #region Rotation

    public void RotatePositive()
    {
        Model.transform.Rotate(rotationSensitivity, 0, 0);
    }

    public void RotateNegative()
    {
        Model.transform.Rotate(-1 * rotationSensitivity, 0, 0);
    }
    #endregion
}
