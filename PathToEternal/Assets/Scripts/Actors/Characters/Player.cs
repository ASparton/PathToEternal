using System.Collections;
using UnityEngine;

public class Player : DynamicActor
{
    [Header("Animations")]
    [SerializeField][Tooltip("Duration of the spawn animation.")]
    private float _spawnFallDuration = 5f;

    #region Delegates

    // Delegate indicating where the player begins to move every time he does
    public delegate void PlayerStartMoving(Cell newCell);
    public static event PlayerStartMoving PlayerStartMovingEvent;

    // Delegate indicating that the player has finished his movement
    public delegate void PlayerFinishedMoving(Cell newCell);
    public static event PlayerFinishedMoving PlayerFinishedMovingEvent;

    private void OnEnable() => EnemyPath.EnemyPAttackedEvent += OnEnemyPAttackFinished;

    private void OnDisable() => EnemyPath.EnemyPAttackedEvent -= OnEnemyPAttackFinished;

    #endregion

    public bool InputsEnabled { get; set; }
    private void Start() => InputsEnabled = true;

    /// <summary>
    ///  Listen and react to the user inputs.
    /// </summary>
    private void Update()
    {
        if (!inMovement && !isRotating && InputsEnabled)
        {
            int xDirection = GetXDirection(), yDirection = GetYDirection();

            // If any direction is pointed
            if (xDirection != 0 || yDirection != 0)
            {
                if (transform.rotation != GetRotationToDirection(xDirection, yDirection))
                    LookInDiretion(xDirection, yDirection);
                else
                    HandleMovement(GetDirection(xDirection, yDirection));
            }
        }
    }

    #region Movements

    /// <summary>
    /// Tries to move to the given direction. If he can it notifies that he moved, otherwise it starts the "Can't move" animation.
    /// </summary>
    /// <param name="movementDirection">The direction to try to move to.</param>
    private void HandleMovement(Direction movementDirection)
    {
        if (!MoveToGridPosition(movementDirection) && !inMovement && !isRotating)
            StartCoroutine(PlayerCantMoveAnimation());
    }

    /// <summary>
    /// Interpolate the position of the player to the given destination cell position.
    /// </summary>
    /// <param name="destinationCell">The destination cell</param>
    /// <returns>Corountine</returns>
    protected override IEnumerator MoveToCell(Cell destinationCell)
    {
        Vector3 finalDestination = new Vector3(GetXDestination(destinationCell), GetYDestination(destinationCell), destinationCell.transform.position.z);
        Vector3 startPosition = transform.position;

        inMovement = true;
        if (AnimationController != null)
            AnimationController.SetBool("isMoving", true);

        // Indicates that the dynamic player has started his movement
        PlayerStartMovingEvent?.Invoke(destinationCell);

        bool cellChanged = false;

        float timeElapsed = 0f;
        while (timeElapsed < _cellTransitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, finalDestination, timeElapsed / _cellTransitionDuration);

            // Cell changing
            if (Vector3.Distance(transform.position, finalDestination) < 0.5f && !cellChanged)
            {
                cellChanged = true;
                ChangeCell(destinationCell);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = finalDestination;
        inMovement = false;
        if (AnimationController != null)
            AnimationController.SetBool("isMoving", false);

        // Indicates that the player has finished his movement
        PlayerFinishedMovingEvent?.Invoke(destinationCell);
    }

    #endregion

    /// <summary>
    /// Activate or deactivate the player weapons.
    /// </summary>
    /// <param name="active">True to activate, false to deactivate.</param>
    public void SetWeaponsActive(bool active)
    {
        gameObject.transform.GetChild(1).GetChild(0).Find("Shield").gameObject.SetActive(active);
        gameObject.transform.GetChild(1).GetChild(0).Find("Weapon").gameObject.SetActive(active);
    }

    #region Direction choice management

    /// <summary>
    /// Determine the direction the player is pointing to on the X axis depending on his inputs.
    /// </summary>
    /// <returns>The direction the player is pointing to on the X axis depending on his inputs.</returns>
    private int GetXDirection()
    {
        int xDirection = 0;

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.A))
            xDirection = GetXDirectionOnKey(KeyCode.Q);

        else if (Input.GetKeyUp(KeyCode.D))
            xDirection = GetXDirectionOnKey(KeyCode.D);

        else if (Input.GetKeyUp(KeyCode.S))
            xDirection = GetXDirectionOnKey(KeyCode.S);

        else if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.W))
            xDirection = GetXDirectionOnKey(KeyCode.Z);

        return xDirection;
    }

    /// <summary>
    /// Determine the direction the player is pointing to on the Y axis depending on his inputs.
    /// </summary>
    /// <returns>The direction the player is pointing to on the Y axis depending on his inputs.</returns>
    private int GetYDirection()
    {
        int yDirection = 0;

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.A))
            yDirection = GetYDirectionOnKey(KeyCode.Q);

        else if (Input.GetKeyUp(KeyCode.D))
            yDirection = GetYDirectionOnKey(KeyCode.D);

        else if (Input.GetKeyUp(KeyCode.S))
            yDirection = GetYDirectionOnKey(KeyCode.S);

        else if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.W))
            yDirection = GetYDirectionOnKey(KeyCode.Z);

        return yDirection;
    }

    /// <summary>
    /// Determine the direction on the x axis depending on the key pressed and the actual rotation of the player.
    /// </summary>
    /// <param name="keyPressed">The key pressed by the player</param>
    /// <returns>The direction on the x axis depending on the key pressed and the actual rotation of the player.</returns>
    private int GetXDirectionOnKey(KeyCode keyPressed)
    {
        if (transform.rotation == Quaternion.AngleAxis(90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return 1;
                case KeyCode.S:
                    return -1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(-90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return -1;
                case KeyCode.S:
                    return 1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(0, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return -1;
                case KeyCode.D:
                    return 1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(180, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return 1;
                case KeyCode.D:
                    return -1;
                default:
                    return 0;
            }
        }
        else
            return 0;
    }

    /// <summary>
    /// Determine the direction on the y axis depending on the key pressed and the actual rotation of the player.
    /// </summary>
    /// <param name="keyPressed">The key pressed by the player</param>
    /// <returns>The direction on the y axis depending on the key pressed and the actual rotation of the player.</returns>
    private int GetYDirectionOnKey(KeyCode keyPressed)
    {
        if (transform.rotation == Quaternion.AngleAxis(90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return 1;
                case KeyCode.D:
                    return -1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(-90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return -1;
                case KeyCode.D:
                    return 1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(0, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return 1;
                case KeyCode.S:
                    return -1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(180, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return -1;
                case KeyCode.S:
                    return 1;
                default:
                    return 0;
            }
        }
        else
            return 0;
    }

    #endregion
    #region Player specific animations

    /// <summary>
    /// Make the player fall down and rotate to the entry cell position given.
    /// </summary>
    /// <param name="entryCellPosition">The entry cell position.</param>
    public void Spawn(Vector3 entryCellPosition)
    {
        StartCoroutine(PlayerSpawnAnimation(entryCellPosition));
    }

    /// <summary>
    /// Make the player rotate and decrease its scale to 0 during the end animation.
    /// </summary>
    /// <param name="endAnimationDuration">The duration of the animation</param>
    /// <param name="usedToSpawn">True if the coroutine in used when spawning the player.</param>
    /// <returns>Coroutine</returns>
    public IEnumerator PlayerDizzyAnimation(float endAnimationDuration)
    {
        // Start animation
        AnimationController.SetBool("isDizzy", true);

        float timeElapsed = 0f;
        while (timeElapsed < endAnimationDuration)
        {
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y + 1f, eulerAngles.z));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Stop animation
        AnimationController.SetBool("isDizzy", false);
    }

    /// <summary>
    /// Make the player fall down to the destination cell position given.
    /// </summary>
    /// <param name="destinationCellPosition">The destination cell position</param>
    /// <returns>Coroutine</returns>
    private IEnumerator PlayerSpawnAnimation(Vector3 destinationCellPosition)
    {
        LevelGrid.Instance.GameInputsEnabled = false;

        // Start animation
        AnimationController.SetBool("isDizzy", true);

        Vector3 startPosition = new Vector3(destinationCellPosition.x, destinationCellPosition.y + 3f, destinationCellPosition.z);

        Quaternion startRotation = Quaternion.AngleAxis(-180f, Vector3.up);
        Quaternion finalRotation = Quaternion.AngleAxis(0, Vector3.up);

        float timeElapsed = 0f;
        while (timeElapsed < _spawnFallDuration)
        {
            transform.position = Vector3.Lerp(startPosition, destinationCellPosition, timeElapsed / _spawnFallDuration);
            transform.rotation = Quaternion.Lerp(startRotation, finalRotation, timeElapsed / _spawnFallDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destinationCellPosition;
        transform.rotation = finalRotation;

        // Stop animation
        AnimationController.SetBool("isDizzy", false);

        LevelGrid.Instance.GameInputsEnabled = true;
    }

    /// <summary>
    /// Play the shaking head no animations for 1 second.
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator PlayerCantMoveAnimation()
    {
        // Start animation
        AnimationController.SetBool("cantMove", true);
        inMovement = true;
        SetWeaponsActive(false);

        yield return new WaitForSeconds(1f);

        // Stop animation
        AnimationController.SetBool("cantMove", false);
        inMovement = false;
        SetWeaponsActive(true);
    }

    /// <summary>
    /// Play the die animation.
    /// </summary>
    /// <param name="enemyCell">The current enemy cell.</param>
    private void OnEnemyPAttackFinished(Cell enemyCell)
    {
        if (AnimationController != null)
        {
            _rotationDuration = 0.5f;
            LookInDiretion(enemyCell.GridPosition.x - Cell.GridPosition.x, enemyCell.GridPosition.y - Cell.GridPosition.y);

            AnimationController.SetBool("die", true);
            Invoke("StopDieAnimation", 1.1f);
        }
    }

    /// <summary>
    /// Stop the die animation, disable game inputs and launch the game over animation.
    /// </summary>
    private void StopDieAnimation()
    {
        AnimationController.SetBool("die", false);
        LevelGrid.Instance.GameInputsEnabled = false;

        GameOverController.Instance.StartAnimation();
    }

    #endregion
}
