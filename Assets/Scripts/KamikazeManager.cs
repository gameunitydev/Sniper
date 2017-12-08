using UnityEngine;

public class KamikazeManager : MonoBehaviour
{
    #region Public Variables



    #endregion

    #region Protected Variables



    #endregion

    #region Private Variables
    
    [SerializeField] private GameObject _destroyedVersion;
    [SerializeField] private GameObject _gun;

    [SerializeField]
    private Transform tr_Player;
    private float f_RotSpeed = 3.0f, f_MoveSpeed = 3.0f;

    private Quaternion _startRotation;

    #endregion

    #region Properties



    #endregion

    #region Unity Methods

    /// <summary>
    /// Is called when the script instance is being loaded
    /// </summary>
    private void Awake()
    {

    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        _startRotation = transform.rotation;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        float step = f_MoveSpeed * Time.deltaTime;
        gameObject.GetComponent<Rigidbody>().AddForce((tr_Player.position - transform.position) * 0.02f);
        transform.position = Vector3.MoveTowards(transform.position, tr_Player.position, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, _startRotation, f_RotSpeed * Time.deltaTime);

        /* Look at Player*/
        /*transform.rotation = Quaternion.Slerp(transform.rotation
            , Quaternion.LookRotation(tr_Player.position - transform.position)
            , f_RotSpeed * Time.deltaTime);*/

        /* Move at Player*/
        //transform.position += transform.forward * f_MoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<DamageManager>().Dead(0);
            Debug.Log("Boom!");
            Destroy(gameObject);
            Time.timeScale = 0;
        }
    }

    #endregion

    #region Methods

    private void OnMouseDown()
    {
        Instantiate(_destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    #endregion
}
