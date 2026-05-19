public class Lumiberry : BaseCollectable
{
    protected override void OnCollected()
    {
        base.OnCollected();
        GameManager.Instance.AddLumiberry();
    }
}