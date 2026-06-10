using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
  [Header("Game objects")]
  [SerializeField] private Transform character;
  [SerializeField] private Transform characterModel;
  [SerializeField] private Transform terrainHolder;
  [SerializeField] private TMPro.TextMeshProUGUI scoreText;

  [Header("Terrain objects")]
  [SerializeField] private Grass grassPrefab;
  [SerializeField] private Road roadPrefab;

  [Header("Game parameters")]
  [SerializeField] private float moveDuration = 0.2f;
  [SerializeField] private int spawnDistance = 20;

  [SerializeField] private GameObject winPanel;
  [SerializeField] private GameObject deadPanel;

  public void PlayerWin() {
    gameState = GameState.Dead;

    winPanel.SetActive(true);
  }


  enum GameState {
    Ready,
    Moving,
    Dead
  }
  private GameState gameState;
  private Vector2Int characterPos;
  private int spawnLocation;
  private List<(float terrainHeight, HashSet<int> locations)> obstacles = new();
  private int score = 0;

  void Awake() {
    NewLevel();
  }

  private void NewLevel() {
    gameState = GameState.Ready;

    Time.timeScale = 1f;
    winPanel.SetActive(false);
    deadPanel.SetActive(false);

    // Reset character position
    characterPos = new Vector2Int(0, -1);
    character.position = new Vector3(0, 0.2f, -1);
    character.GetComponent<Character>().Reset();

    // Reset the score
    score = 0;
    scoreText.text = "0";

    // Remove all terrain
    obstacles.Clear();
    foreach (Transform child in terrainHolder) {
      Destroy(child.gameObject);
    }

    // Reset level, and regenerate
    spawnLocation = 0;
    for (int i = 0; i < spawnDistance; i++) {
      SpawnObstacle();
    }
  }

  private void SpawnObstacle() {
    float roadProbability = Mathf.Lerp(0.5f, 0.9f, spawnLocation / 250f);

    if (Random.value < roadProbability) {
      // Create road with terrain height of 0.1f.
      Road road = Instantiate(roadPrefab, terrainHolder);
      obstacles.Add((0.1f, road.Init(spawnLocation)));
    } else {
      // Create grass with terrain height of 0.2f.
      Grass grass = Instantiate(grassPrefab, terrainHolder);
      obstacles.Add((0.2f, grass.Init(spawnLocation)));
    }

    spawnLocation++;
  }

  private bool InStartArea(Vector2Int location) {
    if ((location.y > -5) && (location.y < 0) && (location.x > -6) && (location.x < 6)) {
      return true;
    }
    return false;
  }


  void Update() {
    if (gameState == GameState.Ready) {
      Vector2Int moveDirection = Vector2Int.zero;
      if (Keyboard.current.upArrowKey.wasPressedThisFrame) {
        character.localRotation = Quaternion.identity;
        moveDirection.y = 1;
      } else if (Keyboard.current.downArrowKey.wasPressedThisFrame) {
        character.localRotation = Quaternion.Euler(0, 180, 0);
        moveDirection.y = -1;
      } else if (Keyboard.current.leftArrowKey.wasPressedThisFrame) {
        character.localRotation = Quaternion.Euler(0, -90, 0);
        moveDirection.x = -1;
      } else if (Keyboard.current.rightArrowKey.wasPressedThisFrame) {
        character.localRotation = Quaternion.Euler(0, 90, 0);
        moveDirection.x = 1;
      }

      if (moveDirection != Vector2Int.zero) {
        Vector2Int destination = characterPos + moveDirection;
        if (InStartArea(destination) || ((destination.y >= 0) && !obstacles[destination.y].locations.Contains(destination.x))) {
          characterPos = destination;
          StartCoroutine(MoveCharacter());
          // Update score if necessary.
          if ((destination.y + 1) > score) {
            score = destination.y + 1;
            scoreText.text = $"{score}";

            if(score >= 50) {
              PlayerWin();
            }
          }
        }

        while (obstacles.Count < (characterPos.y + spawnDistance)) {
          SpawnObstacle();
        }
      }
    }

    if (gameState == GameState.Dead && Keyboard.current.spaceKey.wasPressedThisFrame) {
      NewLevel();
    }

    // Camera follow at (+2, 4, -3)
    Vector3 cameraPosition = new(character.position.x + 2, 4, character.position.z - 3);

    // Limit camera movement in x direction.
    // Only follow the character as it moves to -3 and +3.
    cameraPosition.x = Mathf.Clamp(cameraPosition.x, -1, 5);

    Camera.main.transform.position = cameraPosition;
  }

  private IEnumerator MoveCharacter() {
    gameState = GameState.Moving;
    float elapsedTime = 0f;

    float yHeight = 0.2f;
    if (characterPos.y >= 0) {
      yHeight = obstacles[characterPos.y].terrainHeight;
    }

    Vector3 startPos = character.position;
    Vector3 endPos = new(characterPos.x, yHeight, characterPos.y);

    Quaternion startRotation = characterModel.localRotation;

    while (elapsedTime < moveDuration) {

      float percent = elapsedTime / moveDuration;


      Vector3 newPos = Vector3.Lerp(startPos, endPos, percent);
      // Make the character jump in an arc
      newPos.y = yHeight + (0.5f * Mathf.Sin(Mathf.PI * percent));
      character.position = newPos;


      // Update the elapsed time
      elapsedTime += Time.deltaTime;

      yield return null;
    }

    // Ensure we're at the end.
    character.position = endPos;
    characterModel.localRotation = startRotation;

    if (gameState == GameState.Moving) {
      gameState = GameState.Ready;
    }
  }

  public IEnumerator PlayerCollision() {
    yield return new WaitForSeconds(1f);

    gameState = GameState.Dead;
    deadPanel.SetActive(true);
  }
}
