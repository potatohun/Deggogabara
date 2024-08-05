using UnityEngine;

public class JilFuckSand : MonoBehaviour
{

    public float sinkingPower;
    public float capyMovePOwer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            capyMovePOwer = collision.gameObject.GetComponent<Capybara_Move>().GetMovePower();
            collision.gameObject.GetComponent<Capybara_Move>().SetMovePower(capyMovePOwer - sinkingPower);
            //collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            capyMovePOwer = collision.gameObject.GetComponent<Capybara_Move>().GetMovePower();
            collision.gameObject.GetComponent<Capybara_Move>().SetMovePower(capyMovePOwer + sinkingPower);
            //collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 15f;
        }
    }
}
