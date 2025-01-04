using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private int delayMulai = 1;
    public bool statMulai = false;

    public float laneDistance = 1.5f; // Jarak untuk pindah ke kiri/kanan
    public float jumpForce = 6f;
    public float jumpDuration = 0.2f; // Durasi lompat
    public float slideDuration = 1f;

    private bool isJumping = false;
    private bool isSliding = false;
    private int lane = 1; // 0 = kiri, 1 = tengah, 2 = kanan

    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip jumpSFX;
    public AudioClip hitSFX;
    public AudioClip firstAidSFX; 
    public AudioClip cableSFX; 

    private int coinCount = 0; // Tambahkan variabel untuk menyimpan jumlah koin
    public Text coinText; // Tambahkan referensi ke teks koin

    public float jumpCooldown = 1.5f; // Cooldown time in seconds
    private bool canJump = true; // Boolean to check if the player can jump

    void Start()
    {
        Invoke("mulai_main", delayMulai);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        UpdateCoinUI(); // Inisialisasi teks koin
    }

    private void mulai_main()
    {
        animator.Play("runStart");
        statMulai = true;
    }

void Update()
{
    if (!statMulai) return; // Wait until the game starts

    // Input for moving lanes
    if (Input.GetKeyDown(KeyCode.A))
    {
        if (lane > 0)
        {
            lane--;
            MoveLane();
        }
    }
    else if (Input.GetKeyDown(KeyCode.D))
    {
        if (lane < 2)
        {
            lane++;
            MoveLane();
        }
    }

    // Input for jumping with cooldown check
    if (Input.GetKeyDown(KeyCode.Space) && canJump && !isJumping)
    {
        PlayJumpSFX(); 
        StartCoroutine(Jump());
    }

    // Input for sliding
    if (Input.GetKeyDown(KeyCode.S) && !isSliding)
    {
        StartCoroutine(Slide());
    }
}

    void MoveLane()
    {
        Vector3 targetPosition = transform.position;

        if (lane == 0)
            targetPosition = new Vector3(-laneDistance, transform.position.y, transform.position.z);
        else if (lane == 1)
            targetPosition = new Vector3(0, transform.position.y, transform.position.z);
        else if (lane == 2)
            targetPosition = new Vector3(laneDistance, transform.position.y, transform.position.z);

        // Perpindahan instan tanpa animasi untuk lane kiri/kanan
        transform.position = targetPosition;
    }

IEnumerator Jump()
{
    if (!canJump) yield break; // Exit if jump is on cooldown

    canJump = false; // Set jumping to false to prevent spamming
    isJumping = true;

    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply jump force
    animator.SetBool("Jumping", true); // Trigger jump animation

    // Wait for the jump duration
    yield return new WaitForSeconds(jumpDuration);

    isJumping = false;
    reset_lompat();

    // Wait for the cooldown time
    yield return new WaitForSeconds(jumpCooldown);

    canJump = true; // Allow jumping again
}


    IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("Sliding", true);
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.height /= 2; // Turunkan ukuran collider
        yield return new WaitForSeconds(slideDuration);
        col.height *= 2; // Kembalikan collider ke ukuran normal
        isSliding = false; 
        animator.SetBool("Sliding", false); 
    }

    public void reset_lompat()
    {
        animator.SetBool("Jumping", false); // reset animasi lompat ke kondisi semula
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) 
        {
            animator.SetTrigger("Hit");
            Debug.Log("Obstacle Touched!");

            PlayHitSFX();

            // Efek kamera saat nabrak obstacle
            Camera.main.GetComponent<Kepentok>().StartCoroutine(Camera.main.GetComponent<Kepentok>().Shake(0.3f, 0.2f));

            // Matikan karakter
            statMulai = false;

            // Load Death Screen scene setelah delay
            StartCoroutine(LoadDeathScreen());
        }

        if (other.CompareTag("FirstAid"))
        {
            coinCount++; 
            UpdateCoinUI(); 
            PlayFirstAidSFX(); 
            Destroy(other.gameObject);
        }

        // Jika objek HighObs, play SFX 
        if (other.CompareTag("HighObs"))
        {
            PlayCableSFX(); 
        }
    }

    IEnumerator LoadDeathScreen()
    {
        yield return new WaitForSeconds(1f); 
        SceneManager.LoadScene("Death Screen"); 
    }

    void UpdateCoinUI()
    {
        coinText.text = "Coins: " + coinCount;

        // Update the coins in the GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateCoins(coinCount);
        }
    }


    void PlayJumpSFX()
    {
        if (audioSource != null && jumpSFX != null)
        {
            Debug.Log("Playing JumpSFX");
            audioSource.PlayOneShot(jumpSFX); 
        }
        else
        {
            Debug.LogError("JumpSFX not found!");
        }
    }

    void PlayHitSFX()
    {
        if (audioSource != null && hitSFX != null)
        {
            Debug.Log("Playing HitSFX");
            audioSource.PlayOneShot(hitSFX); 
        }
        else
        {
            Debug.LogError("HitSFX not found!");
        }
    }

    void PlayFirstAidSFX()
    {
        if (audioSource != null && firstAidSFX != null)
        {
            Debug.Log("Playing FirstAidSFX");
            audioSource.PlayOneShot(firstAidSFX); 
        }
        else
        {
            Debug.LogError("FirstAidSFX not found!");
        }
    }

    void PlayCableSFX()
    {
        if (audioSource != null && cableSFX != null)
        {
            Debug.Log("Playing CableSFX");
            audioSource.PlayOneShot(cableSFX); 
        }
        else
        {
            Debug.LogError("CableSFX not found!");
        }
    }

    public int GetCoins()
{
    return coinCount;
}

}
