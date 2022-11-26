using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    public int amount = 1;
    public ParticleSystem cureEffect;
    public AudioClip collectedClip;

    //��Ӵ�������ײ�¼�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if(rubyController != null)
        {
            if(rubyController.health < rubyController.maxHealth)
            {
                rubyController.ChangeHealth(amount);
                Instantiate(cureEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                //���ųԲ�ݮ��Ч
                rubyController.PlaySound(collectedClip);
            }
            else
            {
                Debug.Log("��ǰ�������ֵ������,���ü�Ѫ");
            }
            
        }
        else
        {
            Debug.LogError("rubyController���û�б���ȡ��");
        }
        
    }
}
