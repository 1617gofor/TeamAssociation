using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RobotController : MonoBehaviour
{
    public float speed = 1f;
    public bool vertical;//�����߻��Ǻ�����
    public float changeTime = 3.0f;//��ת���˷���ʱ��
    Rigidbody2D rigidbody2D;
    Vector2 position;
    int direction = 1;//����
    float timer;//��ʱ��

    public AudioClip fixClip;//�޸���Ƶ
    public AudioSource audioSource;

    Animator animator;

    bool broken = true;//�������Ƿ񻵵�
    public ParticleSystem smokeEffect;//������Ч

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        audioSource =  GetComponent<AudioSource>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken)
        {
            return;
        }

        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = changeTime;
            direction = -direction;
        }
    }

    private void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        MovePosition();
    }

    private void MovePosition()
    {
        position = transform.position;
        if (vertical)
        {
            position.y += speed * direction * Time.deltaTime;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x += speed * direction * Time.deltaTime;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
        
        rigidbody2D.position = position;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController player = collision.gameObject.GetComponent<RubyController>();
        if(player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    //�޸�������
    public void Fix()
    {
        broken = false;//�޸�
        audioSource.PlayOneShot(fixClip);//�����޸���Ƶ
        //�û����˲�����ײ
        rigidbody2D.simulated = false;
        
        //����������
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();//ֹͣ������Ч
        audioSource.clip = null;//ֹͣ��·��Ч
    }
}
