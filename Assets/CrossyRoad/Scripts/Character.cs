using UnityEngine;

public class Character : MonoBehaviour {
  [SerializeField] private GameManager gameManager;
  [SerializeField] private GameObject character;
  [SerializeField] private ParticleSystem deathParticles;

  private void OnCollisionEnter(Collision collision) {
    // Only collide with vehicles if we're not already done so.
    if (collision.gameObject.CompareTag("Vehicle") && character.activeSelf) {
      character.SetActive(false);

      Vector3 collisionPoint = collision.GetContact(0).point;
      deathParticles.transform.position = collisionPoint;
      deathParticles.transform.LookAt(transform.position + Vector3.up);

      deathParticles.Play();

      StartCoroutine(gameManager.PlayerCollision());
    }
  }
  public void Reset() {
    character.SetActive(true);
    deathParticles.Clear();
  }
}
