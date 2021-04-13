using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public struct ObjectInfo
{
    public Vector3 Position;
    public Vector3 Velocity;
    public float Mass;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("UI References")]
    [SerializeField]
    private GameObject CoordinatesPrefab;
    [SerializeField]
    private GameObject ObjectPanel;
    [SerializeField]
    private TMP_InputField Mass;
    [SerializeField]
    private TMP_InputField XPosition;
    [SerializeField]
    private TMP_InputField YPosition;
    [SerializeField]
    private TMP_InputField ZPosition;
    [SerializeField]
    private TMP_InputField XVelocity;
    [SerializeField]
    private TMP_InputField YVelocity;
    [SerializeField]
    private TMP_InputField ZVelocity;
    [Header("Inputs")]
    [SerializeField]
    private TMP_InputField TimestepInput;
    [SerializeField]
    private TMP_InputField RenderFPSLimitInput;
    [SerializeField]
    private TMP_InputField TimeScaleInput;
    [SerializeField]
    private TMP_InputField GravitationalConstInput;
    private GameObject CoordinatesObject;
    void Awake()
    {
        Instance = this;
    }
    public void SettingsButtonCallBack()
    {
        GravitationalConstInput.text = GameManager.Instance.GetGravitationalConstant().ToString();
        TimeScaleInput.text = GameManager.Instance.GetTimeScale().ToString();
        RenderFPSLimitInput.text = GameManager.Instance.GetRenderFPSLimit().ToString();
        TimestepInput.text = GameManager.Instance.GetPhysicsFPS().ToString();
    }
    public void SaveButtonCallBack()
    {
        ChangeTimestep();
        UpdateGravitationalConstant();
        UpdateRenderFPSLimit();
        UpdateTimeScale();
    }
    public void ChangeTimestep()
    {
        if(float.Parse(TimestepInput.text) > 0)
            GameManager.Instance.UpdatePhysicsFPS(float.Parse(TimestepInput.text));
    }
    public void UpdateGravitationalConstant()
    {
        if(float.Parse(GravitationalConstInput.text) > 0)
            GameManager.Instance.UpdateGravitationalConstant(float.Parse(GravitationalConstInput.text));
    }
    public void UpdateTimeScale()
    {
        if(float.Parse(TimeScaleInput.text) > 0)
            GameManager.Instance.ChangeTimeScale(float.Parse(TimeScaleInput.text));
    }
    public void UpdateRenderFPSLimit()
    {
        if(int.Parse(RenderFPSLimitInput.text) > 0)
            GameManager.Instance.UpdateRenderFPSLimit(int.Parse(RenderFPSLimitInput.text));
    }
    public void ViewObject(SpaceObject spaceObject)
    {
        ObjectPanel.SetActive(true);
        CoordinatesObject = Instantiate(CoordinatesPrefab,spaceObject.transform.position,Quaternion.identity,spaceObject.transform);
        Mass.text = spaceObject.Mass.ToString();
        XVelocity.text = spaceObject.GetComponent<SpaceObject>().CurrentVelocity.x.ToString();
        YVelocity.text = spaceObject.GetComponent<SpaceObject>().CurrentVelocity.y.ToString();
        ZVelocity.text = spaceObject.GetComponent<SpaceObject>().CurrentVelocity.z.ToString();
        XPosition.text = spaceObject.transform.position.x.ToString();
        YPosition.text = spaceObject.transform.position.y.ToString();
        ZPosition.text = spaceObject.transform.position.z.ToString();
    }
    public void UpdateSpaceObject(SpaceObject spaceObject)
    {
        Mass.text = spaceObject.Mass.ToString();
        XVelocity.text = spaceObject.GetComponent<SpaceObject>().CurrentVelocity.x.ToString();
        YVelocity.text = spaceObject.GetComponent<SpaceObject>().CurrentVelocity.y.ToString();
        ZVelocity.text = spaceObject.GetComponent<SpaceObject>().CurrentVelocity.z.ToString();
        XPosition.text = spaceObject.transform.position.x.ToString();
        YPosition.text = spaceObject.transform.position.y.ToString();
        ZPosition.text = spaceObject.transform.position.z.ToString();
    }
    public ObjectInfo RequestSpaceObjectDetails()
    {
        ObjectInfo info;
        info.Mass = float.Parse(Mass.text);
        info.Velocity = new Vector3(float.Parse(XVelocity.text), float.Parse(YVelocity.text), float.Parse(ZVelocity.text));
        info.Position = new Vector3(float.Parse(XPosition.text), float.Parse(YPosition.text), float.Parse(ZPosition.text));
        return info;
    }
    public void HideObjectPanel()
    {
        ObjectPanel.SetActive(false);
        DestroyCoordinatesObject();
    }
    public void DestroyCoordinatesObject()
    {
        if(CoordinatesObject)
            Destroy(CoordinatesObject);
    }
}