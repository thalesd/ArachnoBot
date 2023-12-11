using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helper;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ControllableArachnoBot : MonoBehaviour, IControllableEntity
    {
        private Rigidbody _rb;
        private SpriteRenderer _spriteRenderer;
        private IControllableMovement _controllableMovement;

        public GameObject eye;
        public GameObject finishLine;
        private Outline outln;

        private GameObject PossessionRadiusHUD;
        private GameObject InteractionRadius;
        private GameObject PossessionCrosshair;
        public float possessionRadius;

        public int jumpForce;

        public float transition;
        private bool isPossessing;
        private bool isDead;

        public bool isOnGrid;
        private bool isOnFloor;

        public bool movementToggle;

        public LayerMask PossessibleLayerMask;
        public LayerMask WhatIsGroundLayer;

        public Animator anim;

        private bool isMouseButtonZeroPressed;

        private GameObject hoveringControllable = null;

        public bool afterAnim;
        private bool flickBool;

        public AudioSource arachnoSound;
        public AudioClip walkingSound, jumpingSound, landingSound, deactivatingSound, activatingSound;

        private bool isInteracting = false;
        private bool isJumping = false;

        private float knockbackForce = 15f;
        private float knockbackTime = 0.5f;
        private bool isKnockedBack = false;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _controllableMovement = new ControllableSimpleMovement();
            arachnoSound = GetComponent<AudioSource>();

            possessionRadius = possessionRadius == 0 ? 10 : possessionRadius;

            PossessionRadiusHUD = transform.Find("PossessionRadiusHUD").gameObject;

            Animator hudAnim = PossessionRadiusHUD.GetComponent<Animator>();
            hudAnim.Play("Base Layer.TurnOn");
            PossessionRadiusHUD.transform.localPosition = Vector3.zero;

            PossessionCrosshair = transform.Find("PossessionCrosshair").gameObject;
            PossessionCrosshair.transform.position = transform.position;

            InteractionRadius = transform.Find("InteractionRadius").gameObject;
        }

        /*public void KnockBack()
        {
            var direction = _rb.velocity.normalized * -1;

            StartCoroutine(CoAwaitKnockBack(direction));
        }*/

        private IEnumerator CoAwaitKnockBack(Vector3 direction)
        {
            isKnockedBack = true;

            _rb.AddForce(knockbackForce * direction, ForceMode.VelocityChange);

            yield return new WaitForSeconds(knockbackTime);

            isKnockedBack = false;
        }

        private void Start()
        {
            AudioListener.volume = 0.8f;

            PossessionRadiusHUD.SetActive(false);
            PossessionCrosshair.SetActive(false);

            SetFeedback();

            if (Player.Instance.possessedObject == gameObject)
            {
                anim.SetBool("isPossessing", false);
            }
            else 
            {
                anim.SetBool("isPossessing", true);
            }

        }

        private void Update()
        {
            if (afterAnim && !isDead && !finishLine.GetComponent<FinishLine>().stageFinished) 
            {
                isMouseButtonZeroPressed = Input.GetMouseButtonDown(0);

                if (isPossessing)
                {
                    CheckForPossessionInput();
                }

                if (_rb.useGravity == false)
                {
                    PossessionRadiusHUD.transform.rotation = Quaternion.Euler(0, 0, 90);
                    PossessionCrosshair.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else 
                {
                    PossessionRadiusHUD.transform.rotation = Quaternion.Euler(0, 0, 0);
                    PossessionCrosshair.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                SetFeedback();

                if (Player.Instance.possessedObject == gameObject)
                {
                    anim.SetBool("isPossessing", false);
                }
                else 
                {
                    anim.SetBool("isPossessing", true);
                }

                if (isJumping && CheckIfGrounded())
                {
                    isJumping = false;
                    anim.SetBool("isJumping", isJumping);
                }
            }

            anim.SetBool("isGrounded", CheckIfGrounded());
            anim.SetFloat("Falling", _rb.velocity.y);
        }

        private void OnTriggerStay(Collider collision)
        {
            if (collision.CompareTag("BackgroundGrid"))
            {
                isOnGrid = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.CompareTag("BackgroundGrid"))
            {
                isOnGrid = false;
            }

            if (collision.CompareTag("Button"))
            {
                collision.GetComponent<Outline>().enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Button"))
            {
                other.GetComponent<Outline>().enabled = true;
            }
        }

        public void Move(Vector2 direction, float speed, float deltaTime)
        {
            if (afterAnim && !isDead && !isKnockedBack && !finishLine.GetComponent<FinishLine>().stageFinished)
            {
                if (!isPossessing)
                {
                    if (!isJumping)
                    {
                        if (isOnGrid && direction.y > 0)
                        {
                            _rb.useGravity = false;
                            SetBoolAnimation("isOnGrid", true);
                            movementToggle = true;
                        }
                        else if (!isOnGrid)
                        {
                            _rb.useGravity = true;
                            SetBoolAnimation("isOnGrid", false);
                            movementToggle = false;
                        }

                        var velocity = direction * speed * deltaTime;

                        if (!movementToggle) velocity.y = _rb.velocity.y;

                        _controllableMovement.Move(_rb, velocity);
                    }

                    UpdateMovementAnimations(direction);
                }
                else
                {
                    if (isOnGrid && movementToggle)
                    {
                        anim.Play("Base Layer.Grid_Idle");
                    }

                    _controllableMovement.Move(_rb, Vector2.zero);
                    UpdateMovementAnimations(Vector2.zero);
                }
            }
            else if (isKnockedBack)
            {
                //Do nothing
            }
            else
            {
                _rb.useGravity = true;
            }
        }

        public void UpdateMovementAnimations(Vector2 direction)
        {
            anim.SetInteger("horizontalMovement", (int)direction.x);
            anim.SetInteger("verticalMovement", (int)direction.y);

            if (direction.magnitude > 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }

        public bool CheckIfGrounded()
        {
            if (!finishLine.GetComponent<FinishLine>().stageFinished)
            {
                return _rb.useGravity && CollisionHelper.CheckIfGrounded(GetComponent<Collider>(), WhatIsGroundLayer);
            }
            else
            {
                return false;
            }
        }

        public void Action()
        {
            if (isPossessing)
            {
                isPossessing = false;
                DeactivatePossessionRadius();
            }
            else
            {
                isPossessing = true;
                ActivatePossessionRadius();
            }
        }

        public void ActivatePossessionRadius()
        {
            CursorController.ChangeCursor(Enums.CursorType.None);

            PossessionRadiusHUD.SetActive(true);

            PossessionCrosshair.transform.position = transform.position;
            PossessionCrosshair.SetActive(true);
        }

        private void CheckForPossessionInput()
        {
            RaycastHit hit;
            Physics.Raycast(PossessionCrosshair.transform.position, Vector3.forward, out hit, Mathf.Infinity, PossessibleLayerMask);

            if (hit.collider != null && hit.collider.gameObject != hoveringControllable)
            {
                //hoveringControllable.StopHighlight();

                hoveringControllable = hit.collider.gameObject;

                if(Player.Instance.possessedObject != hoveringControllable)
                {
                    outln = hoveringControllable.GetComponent<Outline>();

                    outln.enabled = true;
                }

                //hoveringControllable.Highlight(DefaultValues.HighLightColor);
            }
            else if (hit.collider == null)
            {
                if (hoveringControllable != null) 
                {
                    hoveringControllable.GetComponent<IControllableEntity>().StopHighlight();
                    if (Player.Instance.possessedObject != hoveringControllable && hoveringControllable.GetComponent<Outline>())
                    {
                        outln.enabled = false;
                        outln = null;
                    }
                    hoveringControllable = null;

                }
                return;
            }

            if (isMouseButtonZeroPressed && hoveringControllable != null)
            {
                Player.Instance.possessedObject.GetComponent<IControllableEntity>().StopPossessing();
                PlaySound(4);

                Player.Instance.possessedObject = hit.collider.gameObject;

                Player.Instance.possessedObject.GetComponent<IControllableEntity>().Possess();

                anim.SetBool("isMoving", false);
                anim.SetInteger("horizontalMovement", 0);
                anim.SetBool("isJumping", false);

                if (Player.Instance.possessedObject.GetComponent<Outline>()) 
                {
                    outln.enabled = false;
                    outln = null;
                }
                hoveringControllable = null;

                isPossessing = false;

                DeactivatePossessionRadius();

                return;
            }
        }

        public void DeactivatePossessionRadius()
        {
            PossessionRadiusHUD.SetActive(false);
            PossessionCrosshair.SetActive(false);

            CursorController.ChangeCursor(Enums.CursorType.None);
        }

        public void Jump() 
        {
            if (!getIsOnGrid() && CheckIfGrounded())
            {
                _rb.AddForce(new Vector2(0, jumpForce) * Time.fixedDeltaTime, ForceMode.Impulse);
                isJumping = true;
                anim.SetBool("isJumping", isJumping);
                anim.CrossFade("JumpBeginning", transition);
            }
        }

        public void Possess()
        {
            _spriteRenderer.color = DefaultValues.PossessedColor;
        }

        public void SetFeedback()
        {
            StopCoroutine("SetFlickerFeedback");
            eye.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            if (Player.Instance.possessedObject == gameObject)
            {
                eye.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(24f, 0, 0, 1));
            }
            else
            {
                eye.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            }
        }

        public IEnumerator SetFlickerFeedback() 
        {
            eye.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); 
            if (Player.Instance.possessedObject == gameObject)
            {
                if (flickBool)
                {
                    eye.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(24f, 0, 0, 1));
                    flickBool = false;
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    eye.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                    flickBool = true;
                    yield return new WaitForSeconds(0.01f);
                }

                yield return SetFlickerFeedback();
            }
            else
            {
                eye.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            }
        }

        public void DeactivateFeedback() 
        {
            eye.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            if (Player.Instance.possessedObject == gameObject)
            {
                eye.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            }
        }

        public void Highlight(Color highlightColor)
        {
            _spriteRenderer.color = highlightColor;
        }

        public void StopPossessing()
        {
            _spriteRenderer.color = DefaultValues.UnpossessedColor;
        }

        public void StopHighlight()
        {
            _spriteRenderer.color = DefaultValues.UnpossessedColor;
        }

        public float GetPossessionRadius()
        {
            return possessionRadius;
        }

        public bool getIsOnGrid() 
        {
            return isOnGrid;
        }

        public void setIsOnGrid(bool value)
        {
            isOnGrid = value;
        }

        public bool getIsOnFloor()
        {
            return isOnFloor;
        }

        public void SetBoolAnimation(string param, bool value) 
        {
            anim.SetBool(param, value);
        }

        public void SetAfterAnimTrue() 
        {
            afterAnim = true;
        }

        public void PlaySound(int soundValue)
        {
            switch (soundValue)
            {
                case 1:
                    if(arachnoSound.clip != walkingSound)
                    {
                        arachnoSound.clip = walkingSound;
                        arachnoSound.loop = true;
                        arachnoSound.Play();
                    }
                    break;
                case 2:
                    arachnoSound.PlayOneShot(jumpingSound);
                    break;
                case 3:
                    arachnoSound.PlayOneShot(landingSound);
                    break;
                case 4:
                    arachnoSound.PlayOneShot(deactivatingSound);
                    break;
                case 5:
                    arachnoSound.PlayOneShot(activatingSound);
                    break;
            }
        }

        public void StopSound()
        {
            arachnoSound.Stop();
            arachnoSound.clip = null;
        }

        public void Interact()
        {
            if (!isInteracting)
            {
                StartCoroutine(CoInteract());
            }
        }

        IEnumerator CoInteract()
        {
            yield return new WaitForEndOfFrame();

            isInteracting = true;
            InteractionRadius.GetComponent<InteractionController>().setIsPressingToActivate(true);

            yield return null;

            InteractionRadius.GetComponent<InteractionController>().setIsPressingToActivate(false);
            isInteracting = false;
        }

        public bool getIsDead()
        {
            return isDead;
        }

        public void setIsDead()
        {
            isDead = true;
            isOnGrid = false;
            anim.SetBool("isPossessing", true);
            anim.SetBool("isDead", true);
            anim.SetBool("isOnGrid", false);
            DeactivateFeedback();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            if (PossessionCrosshair != null && !finishLine.GetComponent<FinishLine>().stageFinished) 
            {
                Gizmos.DrawSphere(PossessionCrosshair?.transform.position ?? Vector2.zero, .1f);
            }
        }
    }
}
