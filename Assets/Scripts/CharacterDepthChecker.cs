using UnityEngine;

public class CharacterDepthChecker : MonoBehaviour
{
    public GameObject Sprite;
    public Camera MainCam;
    public GameObject Cutout;
    private Vector3 DesiredScale;
    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 RaycastOrigin = Sprite.transform.position;
        bool hasHit = Physics.Raycast(RaycastOrigin, MainCam.transform.position - RaycastOrigin, out hit, Mathf.Infinity);
        Debug.DrawLine(RaycastOrigin, MainCam.transform.position - RaycastOrigin);
        if (hasHit)
        {
            DesiredScale = Vector3.one;
        }
        else
        {
            DesiredScale = Vector3.zero;
        }
        Cutout.transform.localScale = Vector3.Lerp(Cutout.transform.localScale,DesiredScale,15*Time.deltaTime);
    }
}
