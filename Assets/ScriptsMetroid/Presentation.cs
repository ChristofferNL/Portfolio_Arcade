using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Presentation : MonoBehaviour
{
    public List<MeshRenderer> Images = new();
    public Rigidbody Rigidbody;
    public Scene SceneToLoad;
    public MeshRenderer MeshRenderer;
    public int CurrentSlide;
    public bool IsRotating;
    public bool CanWalk;
    public bool CanStartGame;
    public float RotateTime;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartingScreenScene");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeImageForward();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeImageBackwards();
        }
        if (IsRotating && RotateTime > 0.0f)
        {
            RotateTime -= Time.deltaTime;
            if (RotateTime <= 0.0f)
            {
                CanWalk = true;
            }
            RotatePlayer();
        }
        if (CanWalk && Input.GetKey(KeyCode.W))
        {
            Rigidbody.velocity = new Vector3(7, 0, 0);
        }
        else
        {
            Rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
    public void RotatePlayer()
    {
        transform.Rotate(0, + Time.deltaTime * 54, 0);
    }
    public void ChangeImageForward()
    {
        CurrentSlide++;
        if (CurrentSlide < Images.Count)
        {
            MeshRenderer.material = Images[CurrentSlide].material;
        }
        if (CurrentSlide >= Images.Count)
        {
            IsRotating = true;
        }
    }

    public void ChangeImageBackwards()
    {
        if (CurrentSlide > 0)
        {
            CurrentSlide--;
        }
        if (CurrentSlide <= Images.Count)
        {
            MeshRenderer.material = Images[CurrentSlide].material;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Teleporter")
        {
            CanStartGame = true;
        }
    }
}
