using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerArcade : MonoBehaviour
{
	[SerializeField] Rigidbody rb;

	[SerializeField] float _moveSpeed = 5f;
	[SerializeField] Camera _camera;
	[SerializeField] float _maxMoveSpeed = 4f;
	[SerializeField] float _groundDrag = 10;

	[SerializeField] List<MeshRenderer> _arrowMeshes = new();
	[SerializeField] Material _arrowLitMaterial;
	[SerializeField] Material _arrowUnlitMaterial;

	float _horizontalInput;
	float _VerticalInput;

	Vector3 _moveDirection;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		//Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Start()
	{
		rb.drag = _groundDrag;
		rb.useGravity = false;
		if (PersistentUserManager.Instance != null) 
		{
			PersistentUserManager.Instance.ToggleMusic(true);
		}
		StartCoroutine(EntranceArrowsAnimator());
	}

	private void Update()
	{
		MyInput();
		SpeedControl();
		RotatePlayer();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private IEnumerator EntranceArrowsAnimator()
	{
		float timer = 0;
		while (true)
		{
			timer += Time.deltaTime * 3;
			switch (timer)
			{
				case < 1:
					_arrowMeshes[0].material = _arrowLitMaterial;
					_arrowMeshes[1].material = _arrowUnlitMaterial;
					_arrowMeshes[2].material = _arrowUnlitMaterial;
					_arrowMeshes[3].material = _arrowUnlitMaterial;
					break;
				case >= 1 and < 2:
					_arrowMeshes[0].material = _arrowLitMaterial;
					_arrowMeshes[1].material = _arrowLitMaterial;
					_arrowMeshes[2].material = _arrowUnlitMaterial;
					_arrowMeshes[3].material = _arrowUnlitMaterial;
					break;
				case >= 2 and < 3:
					_arrowMeshes[0].material = _arrowLitMaterial;
					_arrowMeshes[1].material = _arrowLitMaterial;
					_arrowMeshes[2].material = _arrowLitMaterial;
					_arrowMeshes[3].material = _arrowUnlitMaterial;
					break;
				case >= 3 and < 4:
					_arrowMeshes[0].material = _arrowLitMaterial;
					_arrowMeshes[1].material = _arrowLitMaterial;
					_arrowMeshes[2].material = _arrowLitMaterial;
					_arrowMeshes[3].material = _arrowLitMaterial;
					break;
				case >= 4:
					timer = 0;
					break;
				default:
					break;
			}
			yield return null;
		}
	}

	private void MyInput()
	{
		_horizontalInput = Input.GetAxisRaw("Horizontal");
		_VerticalInput = Input.GetAxisRaw("Vertical");
	}

	private void Move()
	{
		if (_horizontalInput == 0 && _VerticalInput == 0)
		{
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
			return;
		}
		_moveDirection = transform.forward * _VerticalInput + transform.right * _horizontalInput;

		rb.AddForce(_moveDirection.normalized * _moveSpeed, ForceMode.Force);
	}
	private void SpeedControl()
	{
		Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		float currentMaxSpeed;
		if (transform.localScale.y == 1)
		{
			currentMaxSpeed = _maxMoveSpeed;
		}
		else
		{
			currentMaxSpeed = _maxMoveSpeed / 2;
		}

		if (flatVelocity.magnitude > currentMaxSpeed)
		{
			Vector3 limitedVelocity = flatVelocity.normalized * currentMaxSpeed;

			rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
		}
	}

	private void RotatePlayer()
	{
		transform.rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent<StartGameZone>(out StartGameZone zone)) 
		{
			SceneManager.LoadScene(zone.SceneToOpenName);
		}
	}
}
