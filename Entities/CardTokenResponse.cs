namespace MercadoPagoAPI.Entities;

public class CardTokenResponse
{
    public string Id { get; set; }
    public string FirstSixDigits { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public string LastFourDigits { get; set; }
    public Cardholder Cardholder { get; set; }
    public string Status { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastUpdated { get; set; }
    public DateTime DateDue { get; set; }
    public bool LuhnValidation { get; set; }
    public bool LiveMode { get; set; }
    public bool RequireEsc { get; set; }
    public int CardNumberLength { get; set; }
    public int SecurityCodeLength { get; set; }
}

public class Cardholder
{
    public Identification Identification { get; set; }
    public string Name { get; set; }
}

public class Identification
{
    public string Number { get; set; }
    public string Type { get; set; }
}
