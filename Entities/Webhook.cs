namespace MercadoPagoAPI.Entities;

public class Webhook
{
    public string Action { get; set; }
    public string ApiVersion { get; set; }
    public Data Data { get; set; }
    public string DateCreated { get; set; }
    public string Id { get; set; }
    public bool LiveMode { get; set; }
    public string Type { get; set; }
    public long UserId { get; set; }
}

public class Data
{
    public string Id { get; set; }
}
