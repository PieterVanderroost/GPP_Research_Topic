using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour
{
    [SerializeField] float m_LifeTime = 1.0f;
    private const string KILL = "Kill";
    // Start is called before the first frame update
    void Awake()
    {
        Invoke(KILL, m_LifeTime);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
