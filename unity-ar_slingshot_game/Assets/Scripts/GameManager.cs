using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // public Game Objects
    [Header("Targets")]
    public GameObject targetPrefab;
    public int targetsNum = 5;
    public int ammo = 7;

    [Header("UI Canvas Objects")]
    public GameObject planeSearchingCanvas;
    public GameObject selectPlaneCanvas;
    public GameObject startButton;
    public GameObject gameUI;
    public Text scoreTxt;
    public GameObject ammoImagePrefab;
    public GameObject ammoImageGrid;
    public GameObject playAgainButton;

    [Header("Sounds")]
    public AudioSource EndingSound;
    public AudioSource planeSelectedSound;

    [Header("Scripts")]

    [Header("Materials")]
    public Material PlaneOcclusionMaterial;

    // private variables
    int totalPoints = 0;

    // private GameObjects
    ARPlane selectedPlane = null;    
    ARRaycastManager raycastManager;
    ARPlaneManager planeManager;
    SlingShot slingShot;
    ARSession session;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    Dictionary<int, GameObject> targets = new Dictionary<int, GameObject>();

    //Events
    public delegate void PlaneSelectedEventHandler(ARPlane thePlane);
    public event PlaneSelectedEventHandler OnPlaneSelected;
    void Awake()
    {
        session = FindObjectOfType<ARSession>();
        if (session != null)
        {
            session.Reset();
        }
        else
        {
            Debug.LogWarning("ARSession not found. Ensure an ARSession component is present in the scene.");
        }
    }   
    
    // Start is called before the first frame update
    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager not found in the scene!");
        }

        planeManager = FindObjectOfType<ARPlaneManager>();
        if (planeManager == null)
        {
            Debug.LogError("ARPlaneManager not found in the scene!");
        }
        else
        {
            planeManager.planesChanged += PlanesFound;
        }

        slingShot = FindObjectOfType<SlingShot>();
        if (slingShot == null)
        {
            Debug.LogError("SlingShot not found in the scene!");
        }

        OnPlaneSelected += PlaneSelected;

        // In Play Mode, simulate planes
#if UNITY_EDITOR
        CreateMockPlanes();
#else
        if (planeManager != null)
        {
            planeManager.planesChanged += PlanesFound;
        }
        if (OnPlaneSelected != null)
        {
            OnPlaneSelected += PlaneSelected;
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // Check if in the Unity Editor
#if UNITY_EDITOR
        if (Input.touchCount > 0 && selectedPlane == null && planeManager.trackables.count > 0)
        {
            SelectPlane();
        }
#else
    // Code for when not in Unity Editor (e.g., when on device)
    if (Input.touchCount > 0 && selectedPlane == null && planeManager.trackables.count > 0)
    {
        SelectPlane();
    }
#endif
    }
    
    #if UNITY_EDITOR
        void CreateMockPlanes()
        {
            if (selectedPlane == null)
            {
                // Create a mock AR plane for testing in the Editor
                selectedPlane = new GameObject("Mock AR Plane").AddComponent<ARPlane>();
                selectedPlane.gameObject.AddComponent<LineRenderer>();  // Optional, to improve mock plane appearance
                selectedPlane.GetComponent<Renderer>().material = PlaneOcclusionMaterial;

                // Mock the plane's position and rotation
                selectedPlane.transform.position = new Vector3(0, 0, 2);
                selectedPlane.transform.rotation = Quaternion.identity;

                // Simulate plane selection immediately
                OnPlaneSelected?.Invoke(selectedPlane);

                // Show the select plane canvas after mock plane is created
                planeSearchingCanvas.SetActive(false);
                selectPlaneCanvas.SetActive(true);
            }
        }
    #endif
    
    private void SelectPlane()
    {
        Debug.Log("SelectPlane method called");
        Touch touch = Input.GetTouch(0);
        Debug.Log("Touch phase: " + touch.phase);

        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log("TouchBegan");
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Debug.Log("Raycast hit: " + hits.Count);
                ARRaycastHit hit = hits[0];
                selectedPlane =  planeManager.GetPlane(hit.trackableId);                
                selectedPlane.GetComponent<LineRenderer>().positionCount = 0;
                Debug.Log("Selected plane: " + selectedPlane);
                
                selectedPlane.GetComponent<Renderer>().material = PlaneOcclusionMaterial;
                // SetMaterialTransparent(selectedPlane);
                
                foreach(ARPlane plane in planeManager.trackables)
                {
                    if (plane != selectedPlane)
                    {
                        plane.gameObject.SetActive(false);
                    }
                }
                planeManager.enabled = false;
                selectPlaneCanvas.SetActive(false);
                OnPlaneSelected?.Invoke(selectedPlane);
            }
        }
    }

    void SetMaterialTransparent(ARPlane plane)
    {        
        foreach (Material material in plane.GetComponent<Renderer>().materials)
        {
            material.SetFloat("_Mode", 2);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            // material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }
    void PlanesFound(ARPlanesChangedEventArgs args)
    {
        if (selectedPlane == null && planeManager.trackables.count > 0)
        {
            planeSearchingCanvas.SetActive(false);  // Hides searching canvas
            if (planeSearchingCanvas != null)
            {
                planeSearchingCanvas.SetActive(false);  // Hides searching canvas
            }
            else
            {
                Debug.LogWarning("planeSearchingCanvas is not assigned.");
            }
            selectPlaneCanvas.SetActive(true);     // Show the plane selection canvas
            planeManager.planesChanged -= PlanesFound;  // Unsubscribe to avoid further checks
        }
    }

    void PlaneSelected(ARPlane plane)
    {
        // Hide the "Select Plane" canvas (assumes you have a reference to it)
        selectPlaneCanvas.SetActive(false); // or selectPlaneCanvas.gameObject.SetActive(false);
        
        foreach (KeyValuePair<int, GameObject> target in targets)
        {
            Destroy(target.Value);
        }
        targets.Clear();

        startButton.SetActive(true);
        for (int i = 1; i <= targetsNum; i++)
        {
            GameObject target = Instantiate(targetPrefab, plane.center, plane.transform.rotation, plane.transform);
            target.GetComponent<MoveRandomly>().StartMoving(plane);
            target.GetComponent<Target>().ID = i;
            target.GetComponent<Target>().OnTargetDestroy += UpdateGameWhenHitTarget;
            targets.Add(i, target);
        }
    }

    void UpdateGameWhenHitTarget(int id, int points)
    {
        targets.Remove(id);
        totalPoints += points;
        scoreTxt.text = totalPoints.ToString();
        if (targets.Count == 0)
        {
            ShowPlayAgainButton();
        }
    }
    public void StartGame()
    {
        slingShot.AmmoLeft = ammo;
        slingShot.OnReload += SlingShootReload;
        slingShot.Reload();
        totalPoints = 0;
        scoreTxt.text = totalPoints.ToString();
        startButton.SetActive(false);
        gameUI.SetActive(true);

        

        for (int i = 0; i < slingShot.AmmoLeft; i++)
        {
            GameObject ammoGO = Instantiate(ammoImagePrefab);
            ammoGO.transform.SetParent(ammoImageGrid.transform, false);
        }
    }
    void SlingShootReload(int ammoLeft)
    {
        if (ammoImageGrid.transform.childCount > 0 && ammoLeft >= 0)
        {
            Destroy(ammoImageGrid.transform.GetChild(0).gameObject);
        }
        else if (ammoLeft == 0)
        {
            ShowPlayAgainButton();            
        }
    }
    public void ShowPlayAgainButton()
    {
        EndingSound.Play();
        // gameUI.SetActive(false);
        foreach (Transform ammoImge in ammoImageGrid.transform)
        {
            Destroy(ammoImge.gameObject);
        }
        slingShot.Clear();
        slingShot.OnReload -= SlingShootReload;
        playAgainButton.SetActive(true);
    }
    
    public void PlayAgain()
    {
        PlaneSelected(selectedPlane);
        EndingSound.Stop();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("ARSlingshotGame");
    }
}
