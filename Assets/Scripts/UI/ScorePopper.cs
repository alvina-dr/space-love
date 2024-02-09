using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScorePopper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private void Start()
    {
        textMesh.transform.DOScale(1.3f, .2f).OnComplete(() =>
        {
            textMesh.transform.DOScale(1f, .1f);
        });
        textMesh.transform.DOMoveY(textMesh.transform.position.y + 30, .3f).OnComplete(() =>
        {
            Destroy(gameObject);
        });      
    }

    public void SetupScorePopper(int _score, Vector3 _worldPosition)
    {
        textMesh.text = _score.ToString();
        transform.position = Camera.main.WorldToScreenPoint(_worldPosition);
        if (_score >= 40) transform.localScale *= 2;
        else if (_score >= 20) transform.localScale *= 1.5f;
    }
}
