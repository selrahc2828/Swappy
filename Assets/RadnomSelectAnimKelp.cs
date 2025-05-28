using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RadnomSelectAnimKelp : MonoBehaviour
{
    private Animator animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("RandomFollowAnim", Random.RandomRange(0, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
