using TMPro;
using UnityEngine;

public class NonePlayerCharacter : MonoBehaviour
{
    //�Ի�����ʾʱ��
    public float displayTime = 4.0f;
    //������ȡ�Ի���
    public GameObject dialogBox;
    //��ʱ��
    float timerDisplay;
    //������Ϸ�����ȡTMP�ؼ�
    public GameObject TMPGameObject;
    TextMeshProUGUI _tmTexBox;
    //�洢ҳ��
    int _currentPage = 1;
    int _totalPages;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        _tmTexBox = TMPGameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _totalPages = _tmTexBox.textInfo.pageCount;
        if (timerDisplay >= 0.0f)
        {
            //��ҳ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(_currentPage < _totalPages)
                {
                    _currentPage += 1;
                }
                else
                {
                    _currentPage = 1;
                }
            }
            _tmTexBox.pageToDisplay = _currentPage;
            timerDisplay -= Time.deltaTime;
        }
        else
        {
            dialogBox.SetActive(false);
        }

    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}

