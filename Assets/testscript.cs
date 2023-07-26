using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class testscript : MonoBehaviour
{
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _offsetX;

	private RectTransform _rectTransform;

	private Vector3 _originPosition;
	private Vector3 _targetPosition;

	private Vector3 _mouseOriginPosition;

	private bool _isDraggable = true;
	private bool _isActivated;

	private void Start()
	{
		_rectTransform = GetComponent<RectTransform>();
		_originPosition = _rectTransform.position;
		Application.targetFrameRate = 30;
	}

	private void Update()
	{
		if (_isActivated && _isDraggable)
		{
			_targetPosition = _originPosition + (Input.mousePosition - _mouseOriginPosition);
			StartCoroutine(MoveElement(_rectTransform.position, _targetPosition, _moveSpeed, true));
		}
	}

	public void ActivateElement()
	{
		if (_isDraggable)
		{
			_isActivated = true;
			_mouseOriginPosition = Input.mousePosition;
		}
	}

	public void DeactivateElement()
	{
		if (_isActivated && _isDraggable)
		{
			if (_mouseOriginPosition.x < Input.mousePosition.x && _mouseOriginPosition.x + _offsetX < Input.mousePosition.x)
			{
				_originPosition = new Vector3(_originPosition.x + _rectTransform.rect.width, _rectTransform.position.y, _rectTransform.position.z);
			}
			else if (_mouseOriginPosition.x > Input.mousePosition.x && _mouseOriginPosition.x - _offsetX > Input.mousePosition.x)
			{
				_originPosition = new Vector3(_originPosition.x - _rectTransform.rect.width, _rectTransform.position.y, _rectTransform.position.z);
			}
			StartCoroutine(MoveElement(_rectTransform.position, _originPosition, _moveSpeed, false));
		}
	}
		
	private IEnumerator MoveElement(Vector3 startPosition, Vector3 endPosition, float speed, bool oneTimeMove)
	{
		_rectTransform.position = new Vector3(Mathf.Lerp(_rectTransform.position.x, endPosition.x, speed), _rectTransform.position.y, _rectTransform.position.z);
		if (oneTimeMove)
		{
			yield break;
		}
		_isDraggable = false;
		while (Vector3.Distance(_rectTransform.position, endPosition) > 0.5f)
		{
			_rectTransform.position = new Vector3(Mathf.Lerp(_rectTransform.position.x, endPosition.x, speed), _rectTransform.position.y, _rectTransform.position.z);
			speed += 0.1f;
			yield return null;
		}
		_isDraggable = true;
		_isActivated = false;
	}
}
