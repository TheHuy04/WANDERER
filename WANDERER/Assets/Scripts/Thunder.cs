using System.Collections;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private Animator ThunderAnimator;  // Animator của tia sét
    public AudioSource thunderSound;    // Âm thanh sấm sét
    public float minDelay = 5f;         // Thời gian trễ tối thiểu giữa các tia sét
    public float maxDelay = 10f;        // Thời gian trễ tối đa giữa các tia sét

    private void Start()
    {
        // Bắt đầu coroutine để kích hoạt tia sét
        ThunderAnimator = GetComponent<Animator>();
        StartCoroutine(TriggerLightning());
    }

    private IEnumerator TriggerLightning()
    {
        while (true)
        {
            // Chờ một khoảng thời gian ngẫu nhiên giữa minDelay và maxDelay
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            // Phát animation tia sét
            ThunderAnimator.SetTrigger("Thunder");

            // Phát âm thanh sấm sét
            thunderSound.Play();
        }
    }
}
