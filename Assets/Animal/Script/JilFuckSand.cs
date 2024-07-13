using UnityEngine;

public class JilFuckSand : MonoBehaviour
{

    public float sinkingPower;
    public float capyMovePOwer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            capyMovePOwer = collision.gameObject.GetComponent<Capybara_Move>().GetMovePower();
            collision.gameObject.GetComponent<Capybara_Move>().SetMovePower(capyMovePOwer - sinkingPower);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            capyMovePOwer = collision.gameObject.GetComponent<Capybara_Move>().GetMovePower();
            collision.gameObject.GetComponent<Capybara_Move>().SetMovePower(capyMovePOwer + sinkingPower);
        }
    }




}
