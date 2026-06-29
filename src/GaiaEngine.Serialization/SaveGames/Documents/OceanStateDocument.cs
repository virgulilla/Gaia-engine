namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized optional ocean section inside a water document.
/// </summary>
internal sealed class OceanStateDocument
{
    public int SeaLevel { get; set; }

    public int Salinity { get; set; }

    public int Temperature { get; set; }
}
