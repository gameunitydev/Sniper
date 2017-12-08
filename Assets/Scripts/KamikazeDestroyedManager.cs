using UnityEngine;

public class KamikazeDestroyedManager : MonoBehaviour
{
    #region Public Variables



    #endregion

    #region Protected Variables



    #endregion

    #region Private Variables

    [SerializeField] private float _delay;

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
        Destroy(gameObject, _delay);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {

    }

    #endregion

    #region Methods



    #endregion
}
