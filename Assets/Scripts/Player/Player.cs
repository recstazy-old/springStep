using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class Player : GameSys
    {
        public static event InputHandler.TapHandler OnAddStepCount;

        public GameObject freeEnd, fixedEnd;
        ChainEnd free, fixd;

        bool goUp = false;
        public static bool moving = false;
        public static bool firstStep = false;

        public static Vector2 normalVector = Vector2.up; // Platform orientation, will be changed, if you trig to vertical platform

        void Start()
        {
            free = freeEnd.GetComponent<ChainEnd>();
            fixd = fixedEnd.GetComponent<ChainEnd>();

            InputHandler.OnTap += StartMovingUp;
            InputHandler.OnTap2 += ToFall;
            InputHandler.OnTap3 += ChangeDirection;
            ChainEnd.OnPull += PullFixedEnd;
            ChainEnd.OnRestoreDefaults += RestoreDefaults;
            ChainEnd.OnGameOver += GameOver;
        }

        private void ChangeDirection()
        {
            free.Rigidbody.velocity = Vector2.down * free.Rigidbody.velocity.magnitude;
        }

        void RestoreDefaults()
        {
            normalVector = free.platform.up;

            if (!restarting)
            {
                SwitchEnds();
            }

            moving = false;
            goUp = false;
            firstStep = false;
            restarting = false;
            OnAddStepCount?.Invoke();
        }
        
        // When the step is done - switch free and fixed ends references, 
        // so the step cycle becomes smaller twice

        void SwitchEnds() 
        {
            GameObject temp0 = freeEnd;
            ChainEnd temp1 = free;

            freeEnd = fixedEnd;
            fixedEnd = temp0;

            free = fixd;
            fixd = temp1;
        }

        private void StartMovingUp()
        {
            moving = true;
            firstStep = true;
            //StartCoroutine(MoveUp());

            free.Rigidbody.velocity = normalVector * 7;
        }

        IEnumerator MoveUp()
        {
            // Moves a free end up relative to platform orientation

            goUp = true;
            firstStep = true;
            
            while (goUp)
            {
                free.Rigidbody.MovePosition(free.Rigidbody.position + normalVector * 7 * Time.deltaTime); 
                yield return null;
            }
        }

        private void ToFall()
        {
            // Kicks the free end to right relative to platform orientation

            goUp = false;
            free.Rigidbody.velocity = - Vector2.Perpendicular(normalVector) * (Vector2.Distance(free.transform.position, fixd.transform.position)* 2f);

            StartCoroutine(Fall(free));
        }

        IEnumerator Fall(ChainEnd end)
        {
            // Adds a kinematic "gravity" to free end when it's already kicked, relative to platform orientation

            while (Mathf.Abs(Vector2.SignedAngle(end.Rigidbody.velocity, normalVector)) > 10f)
            {
                end.Rigidbody.velocity += - normalVector * 0.5f + Vector2.Perpendicular(normalVector) * 0.1f;
                yield return null;
            }

            end.Rigidbody.velocity = -normalVector * end.Rigidbody.velocity.magnitude;
        }

        void PullFixedEnd()
        {
            firstStep = false;

            free.SpawnTarget();
            fixd.Rigidbody.velocity = normalVector*20;
            
            StartCoroutine(PullFixed());
        }

        IEnumerator PullFixed()
        {
            // Calculates the circle between target position for the second end and moves it by it's radius

            Vector2 target = free.platform.position + free.platform.up * 3;
            Vector2 direction = target - fixd.Rigidbody.position;
            Vector2 center = fixd.Rigidbody.position + direction / 2;

            while (moving)
            {
                fixd.Rigidbody.velocity += center - fixd.Rigidbody.position;
                yield return null;
            }
        }

        private void Update() //Need to rotate ends in velosity direction, so it looks like they have physics
        {
            if (fixd.Rigidbody.velocity != Vector2.zero)
            {
                fixedEnd.transform.up = Vector2.Lerp(fixedEnd.transform.up, -fixd.Rigidbody.velocity, 0.5f * Time.deltaTime);
            }

            if (free.Rigidbody.velocity != Vector2.zero)
            {
                freeEnd.transform.up = Vector2.Lerp(freeEnd.transform.up, free.Rigidbody.velocity, 0.5f * Time.deltaTime);
            }
        }

        public override void GameOver()
        {
            free.Rigidbody.velocity = Vector2.zero;
        }

        public override void Restart()
        {
            RestoreDefaults();

            if (freeEnd.gameObject.name == "Bottom")
            {
                SwitchEnds();
            }

            normalVector = Vector2.up;
        }
    }
}
