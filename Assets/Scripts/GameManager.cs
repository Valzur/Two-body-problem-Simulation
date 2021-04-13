using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HashSet<SpaceObject> Bodies = new HashSet<SpaceObject>();
    [SerializeField]
    private float GravitationalConstant;
    [SerializeField]
    private GameObject PlanetPrefab;
    private GameObject VisiblePlanet;
    private Queue<SpaceObject>PlanetsToDelete = new Queue<SpaceObject>();
    private bool ToggleVisiblePlanet = false;
    void Awake()
    {
        Instance = this;
        VisiblePlanet = null;
    }
    void FixedUpdate()
    {
        DeleteAllPlanets();
        DoPhysics();
        DelayedTogglePlanetVisibility();
    }
    void Update()
    {
        CheckMouseInput();
    }
    public void UpdateObjectPanel()
    {
        if(VisiblePlanet)
            UIManager.Instance.UpdateSpaceObject(VisiblePlanet.GetComponent<SpaceObject>());
    }    
    private void CheckMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit Hit))
            {
                SpaceObject Object = Hit.transform.GetComponent<SpaceObject>();
                if(Object)
                {
                    Debug.Log(VisiblePlanet);
                    if(VisiblePlanet)
                    {
                        TogglePlanetVisibility();
                        DelayedTogglePlanetVisibility();
                    }
                    UIManager.Instance.ViewObject(Object);
                    Object.GetComponent<Renderer>().material.SetInt("_isSelected",1);
                    VisiblePlanet = Object.gameObject;
                }
            }
        }
    }
    private void DoPhysics()
    {
        if(Bodies.Count <=1)
            return;
        foreach (var Body1 in Bodies)
        {
            foreach (var Body2 in Bodies)
            {
                if(Body1 != Body2)
                {
                    //Direction
                    Vector3 direction = (Body2.transform.position - Body1.transform.position).normalized;
                    // G*m1*m2 /R^2
                    Vector3 velocity = direction * GravitationalConstant * Body1.Mass * Body2.Mass / ( (Mathf.Pow(Vector3.Distance(Body1.transform.position,Body2.transform.position),2)) ) * Time.fixedDeltaTime;
                    Body1.CurrentVelocity += velocity ;
                }
            }
            if(Physics.SphereCast(Body1.transform.position,1, Body1.CurrentVelocity,out RaycastHit raycastHit,Time.fixedDeltaTime))
            {
                Body1.transform.position = (raycastHit.transform.position - Body1.transform.position).normalized * 0.5f;
            }else
            {
                Body1.transform.position += Body1.CurrentVelocity * Time.fixedDeltaTime;
            }
                
        }
    }
    public float GetGravitationalConstant()
    {
        return GravitationalConstant;
    }
    public int GetRenderFPSLimit()
    {
        return Application.targetFrameRate;
    }
    public float GetPhysicsFPS()
    {
        return Time.fixedDeltaTime;
    }
    public float GetTimeScale()
    {
        return Time.timeScale;
    }
    public void UpdateGravitationalConstant(float G)
    {
        GravitationalConstant = G;
    }
    public void UpdateRenderFPSLimit(int FPSLimit)
    {
        Application.targetFrameRate = FPSLimit;
    }
    public void UpdatePhysicsFPS(float FPS)
    {
        Time.fixedDeltaTime = (float)1/FPS;
    }
    public void ChangeTimeScale(float timeScale)
    {
        if(timeScale <= 0)
            return;
        
        Time.timeScale = timeScale;
    }
    public void CreatePlanet()
    {
        GameObject planet = Instantiate(PlanetPrefab,Camera.main.transform.position,Quaternion.identity);
        Color RandColor = Random.ColorHSV();
        planet.GetComponent<Renderer>().material.SetColor("_baseColor", RandColor);
        planet.GetComponent<TrailRenderer>().material.SetColor("_emissionMap", RandColor * 2);
        VisiblePlanet = planet;
    }
    private void DeleteAllPlanets()
    {
        for (int i= 0; i< PlanetsToDelete.Count; i++)
        {
            var planet = PlanetsToDelete.Dequeue();
            Bodies.Remove(planet.gameObject.GetComponent<SpaceObject>());
            planet.gameObject.GetComponent<SpaceObject>().Destroy();
        }
    }
    public void DeletePlanet()
    {
        if(VisiblePlanet == null)
            return;
        PlanetsToDelete.Enqueue(VisiblePlanet.gameObject.GetComponent<SpaceObject>());
        VisiblePlanet = null;
        UIManager.Instance.HideObjectPanel();
    }
    private void DelayedTogglePlanetVisibility()
    {
        if(!ToggleVisiblePlanet)
            return;
        if(!VisiblePlanet)
            ToggleVisiblePlanet = false;
        UIManager.Instance.DestroyCoordinatesObject();
        VisiblePlanet.GetComponent<Renderer>().material.SetInt("_isSelected",0);
        VisiblePlanet = null; 
        ToggleVisiblePlanet = false;
    }
    public void TogglePlanetVisibility()
    {
        if(VisiblePlanet)
        {
            ToggleVisiblePlanet = true;
        }
    }
    public void UpdateObject()
    {
        if(!VisiblePlanet)
            return;
        ObjectInfo info = UIManager.Instance.RequestSpaceObjectDetails();
        VisiblePlanet.transform.position = info.Position;
        VisiblePlanet.GetComponent<SpaceObject>().CurrentVelocity = info.Velocity;
        VisiblePlanet.GetComponent<SpaceObject>().Mass = info.Mass;
    }
}