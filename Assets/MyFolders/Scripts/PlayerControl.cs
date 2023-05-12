using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private Vector2 touchPosition;
    private Vector3 direction;
    private Quaternion targetRotation;

    private new Transform transform;
    private Rigidbody rb;  

    int[] items = new int[16]; 
    private int coins = 100;

    private bool isInShop;

    private Vector3 cameraOffset = new Vector3(0, 2.6f, -2.5f);
    private Transform cameraTransform;

    [SerializeField] private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        cameraTransform = Camera.main.GetComponent<Transform>();
    }

    void Update()
    {
        TouchControll();
        KeyControll();
    }

    void TouchControll()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (isInShop || EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

                anim.SetBool("IsRun", true);
                touchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (isInShop || EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

                direction = touch.position - touchPosition;
                direction.Normalize();

                if (direction.y > 0)
                {
                    anim.SetBool("RunDirection", false);
                    direction = transform.forward * Mathf.Clamp(direction.magnitude, 0, 1);
                }
                else if (direction.y < 0)
                {
                    anim.SetBool("RunDirection", true);
                    direction = -transform.forward * Mathf.Clamp(direction.magnitude, 0, 1);
                }

                float angle = (touch.position.x - Screen.width) * rotationSpeed / 2;
                targetRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                direction = Vector3.zero;
                anim.SetBool("IsRun", false);
            }
        }
    }

    void KeyControll()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction = transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = transform.forward * -1;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("RunDirection", false);
            anim.SetBool("IsRun", true);
            
        }       
        else if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("RunDirection", true);
            anim.SetBool("IsRun", true);           
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log(1);
            direction = Vector3.zero;
            anim.SetBool("IsRun", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            targetRotation = Quaternion.Euler(0f, -rotationSpeed * 500 * Time.deltaTime, 0f) * rb.rotation;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            targetRotation = Quaternion.Euler(0f, rotationSpeed * 500 * Time.deltaTime, 0f) * rb.rotation;
        }


    }

    void FixedUpdate()
    {
        
        if (direction.magnitude > 0)
        {
            Vector3 pos = transform.position + speed * Time.fixedDeltaTime * direction;
            rb.MovePosition(pos);
        }
        if (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000 * Time.fixedDeltaTime);
        }

        
    }

    public bool Buy(int itemIndex, int itemCost)
    {
        if (coins >= itemCost)
        {
            items[itemIndex] ++ ;
            coins -= itemCost;
            return true;
        }
        return false;
    }
    internal bool Sell(int itemIndex, int itemCost)
    {
        if(items[itemIndex]>0)
        {
            items[itemIndex]--;
            coins += itemCost;
            return true;
        }
        return false;

    }
    public int GetCoins()
    {
        return coins;
    }

    public int[] GetItemsCount()
    {
        return items;
    }

    public void IsInShopSet(bool isInShop)
    {
        this.isInShop = isInShop;
    }

    public void MoveCameraBack()
    {
        cameraTransform.parent = transform;
        cameraTransform.DOLocalMove(cameraOffset, 0.5f);
        cameraTransform.DOLocalRotate(new Vector3(34, 0, 0), 0.5f);
    }

    public void PlayWaveAnimation()
    {
        anim.SetTrigger("Wave");
    }
}
