using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerArcade : MonoBehaviour
{
	public enum InfoPages
	{
		CV,
		TETRIS,
		FEJKTROID,
		BIRDSNEST,
		GOLEMGAME,
		CHESS
	}

	[SerializeField] Rigidbody rb;

	[SerializeField] float _moveSpeed = 5f;
	[SerializeField] Camera _camera;
	[SerializeField] float _maxMoveSpeed = 4f;
	[SerializeField] float _groundDrag = 10;

	[SerializeField] List<MeshRenderer> _arrowMeshes = new();
	[SerializeField] Material _arrowLitMaterial;
	[SerializeField] Material _arrowUnlitMaterial;

	[SerializeField] CinemachineVirtualCamera _virtualCamera;
	[SerializeField] Canvas _cvCanvas;
	[SerializeField] Canvas _arcadeCanvas;

	[SerializeField] List<Image> _infoImages = new();
	[SerializeField] TextMeshProUGUI _pressToPlayText;

	float _horizontalInput;
	float _VerticalInput;

	Vector3 _moveDirection;

	bool _isReading;

	InfoPages _pages;
	string _currentSceneToLoad = "";

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
		if (Input.GetKeyDown(KeyCode.Tab)) ToggleReading(_pages);
		if (Input.GetKeyDown(KeyCode.E)) PlayGame();
	}

	private void ToggleReading(InfoPages pages)
	{
		if (_isReading)
		{
			_isReading = false;
			_cvCanvas.gameObject.SetActive(false);
			_arcadeCanvas.gameObject.SetActive(true);
			Cursor.visible = false;
		}
		else
		{
			_isReading = true;
			_cvCanvas.gameObject.SetActive(true);
			_arcadeCanvas.gameObject.SetActive(false);
			Cursor.visible = true;
		}
	}

	private void PlayGame()
	{
		if (_currentSceneToLoad.Length > 1 && !_isReading)
		{
			SceneManager.LoadScene(_currentSceneToLoad);
		}
	}

	private void Move()
	{
		if (_horizontalInput == 0 && _VerticalInput == 0 || _isReading)
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
		if (_isReading) 
		{
			_virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
			_virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
		}
		else
		{
			_virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 350;
			_virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 350;
			transform.rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent<StartGameZone>(out StartGameZone zone)) 
		{
			_pages = zone.Pages;
			_currentSceneToLoad = zone.SceneToOpenName;
			_pressToPlayText.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		_pages = InfoPages.CV;
		_currentSceneToLoad = "";
		_pressToPlayText.gameObject.SetActive(false);
	}
}
