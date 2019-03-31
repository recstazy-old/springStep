using UnityEngine;
using System.Collections;

// The same script on both ends of player

namespace Player
{
    public class ChainEnd : GameSys
    {
        // Use Input delegate, because don't want to create one more void delegate

        public static event InputHandler.TapHandler OnPull;
        public static event InputHandler.TapHandler OnRestoreDefaults; 
        public static event InputHandler.TapHandler OnGameOver;

        public Rigidbody2D Rigidbody { get; private set; }
        public FixedJoint2D FixedJoint { get; private set; }
        Vector2 StartPosition { get; set; }
        Vector2 StartRotation { get; set; }

        public GameObject target; // Needed to stop second end in right place
        public Transform platform;

        void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            FixedJoint = GetComponent<FixedJoint2D>();

            StartPosition = Rigidbody.position;
            StartRotation = transform.up;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Player.firstStep && other.tag == "Platform")
            {
                Platform platformScript = other.GetComponent<Platform>();

                if (!platformScript.Used)
                {
                    platform = other.transform;
                    platformScript.Moving = false;

                    Rigidbody.velocity = Vector2.zero;
                    StartCoroutine(FixPosition(other.transform.position + other.transform.up * 0.8f, -other.transform.up));

                    if (Player.moving)
                    {
                        OnPull?.Invoke();
                    }

                    platformScript.Used = true;
                }
            }

            if (other.tag == "Target")
            {
                Player.moving = false;
                Rigidbody.velocity = Vector2.zero;

                StartCoroutine(FixPosition(other.transform.position, -other.transform.up));

                other.gameObject.SetActive(false);
                OnRestoreDefaults?.Invoke();
            }

            if (other.tag == "GameOver")
            {
                OnGameOver?.Invoke();
            }

            if (other.tag == "BounceEdge")
            {
                Rigidbody.velocity = -Rigidbody.velocity;
            }
        }

        public void SpawnTarget() //When free end falls on platform, it makes a target for the second end
        {
            target.transform.position = platform.position + platform.up * 4;
            target.transform.up = -platform.up;
            target.SetActive(true);
        }

        IEnumerator FixPosition(Vector2 target, Vector2 targetUp)
        {
            while (Vector2.Distance(Rigidbody.position, target) > 0.05f)
            {
                Rigidbody.MovePosition(Vector2.Lerp(Rigidbody.position, target, 0.5f));
                transform.up = Vector2.Lerp(transform.up, targetUp, 0.5f);
                yield return null;
            }
            Rigidbody.MovePosition(target);
            transform.up = targetUp;
        }

        public override void Restart()
        {
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.MovePosition(StartPosition);
            transform.up = StartRotation;
        }

        public override void GameOver()
        {
            Rigidbody.velocity = Vector2.zero;
        }
    }
}
