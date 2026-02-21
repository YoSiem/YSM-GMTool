namespace App.Core.Models;

public sealed class DatabaseConnectionSettings
{
    public string Server { get; set; } = "127.0.0.1";

    public int Port { get; set; } = 1433;

    public string Database { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool IntegratedSecurity { get; set; }

    public bool Encrypt { get; set; } = true;

    public bool TrustServerCertificate { get; set; } = true;

    public DatabaseConnectionSettings Clone() => new()
    {
        Server = Server,
        Port = Port,
        Database = Database,
        UserId = UserId,
        Password = Password,
        IntegratedSecurity = IntegratedSecurity,
        Encrypt = Encrypt,
        TrustServerCertificate = TrustServerCertificate
    };
}
