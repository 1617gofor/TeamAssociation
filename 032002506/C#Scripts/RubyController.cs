using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;//�������ֵ
    int currentHealth;//��ǰ����ֵ
    public float timeOfInvincible = 2.0f;//�޵�ʱ��
    bool isInvincible;//�Ƿ��޵б�־
    float invincibleTime = 2f;//�޵�ʱ���ʱ��
    public int health
    {
        get { return currentHealth; }
        //set { currentHealth = value; }
    }
    

    float horizontal;
    float vertical;
    public float speed = 3.0f;
    Rigidbody2D rigidbody2D;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);//��ֹʱʸ��
    Vector2 move;//�ƶ�ʸ��

    public GameObject projectilePrefab;//�ӵ�Ԥ�Ƽ�

    //��ƵԴ����
    AudioSource audioSource;
    public AudioClip hitClip;//������Ƶ
    public AudioClip throwClip;//���������������

    // Start is called before the first frame update
    void Start()
    {
        /*
        QualitySettings.vSyncCount = 0;//��ֱͬ����������Ϊ0������֡
        Application.targetFrameRate = 10;//Ŀ��֡��=10
        */
        //��ȡ��ǰ��Ϸ����ĸ������
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();//�������
        audioSource = GetComponent<AudioSource>();  

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //������άʸ�����洢ruby����
        move = new Vector2(horizontal, vertical);
        //�����ƶ���
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();//����������Ϊ1
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);//magnitue���Է���ʸ������

        //�����޵�״̬
        if (isInvincible)
        {
            invincibleTime -= Time.deltaTime;
            if(invincibleTime < 0)
            {
                isInvincible = false;
            }
        }

        //��ӷ����ӵ��߼�
        if (Input.GetKeyDown(KeyCode.K) || Input.GetAxis("Fire1")!=0)
        {
            Launch();
        }

        //NPC�����߼�
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC")); 
            if(hit.collider != null)
            {
                Debug.Log($"����������{hit.collider.gameObject}");
            }
            NonePlayerCharacter npc = hit.collider.GetComponent<NonePlayerCharacter>();
            if(npc != null)
            {
                npc.DisplayDialog();
            }

        }
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2D.position = position;
    }

    internal void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTime = timeOfInvincible;
        }
        //����ֵ����0��maxHealth֮��
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        if(amount < 0)
        {
            //�������˶���
            animator.SetTrigger("Hit");
            //����������Ƶ
            PlaySound(hitClip);
        }
        
        Debug.LogFormat("��ǰ����ֵ��{0}/{1}", currentHealth, maxHealth);
        //����Ѫ��
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }
    //�����ӵ�
    private void Launch()
    {
        //ʵ�����ӵ�
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);//ͨ���ű���������ӵ��ƶ�����
        animator.SetTrigger("Launch");//�����ӵ�����
        PlaySound(throwClip);//������Ƶ
    }

    //������Ƶ����
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
