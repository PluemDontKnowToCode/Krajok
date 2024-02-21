using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent (typeof (Rigidbody2D), typeof(BoxCollider2D))]
public class InGameObject : MonoBehaviour 
{
    protected Rigidbody2D rb;
    enum Dimension
    {
        SoulDimension,
        RealDimension,
        Both
    }
    [SerializeField] Dimension SelectDimension;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.layer = 6;
        switch (SelectDimension)
        {
            case Dimension.SoulDimension:
                this.tag = "SoulDimension";
                break;
            case Dimension.RealDimension:
                this.tag = "RealDimension";
                break;
            case Dimension.Both: 
                break;
        }
    }
    protected void ForSomePlatform()
    {
        GetComponent<BoxCollider2D>().usedByEffector = true;
        GetComponent<PlatformEffector2D>().surfaceArc = 180f;
        
    }

}
