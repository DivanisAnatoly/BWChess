using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class ForsythEdwardsNotation
{
    public string PiecePosition { get; set; }

    [JsonProperty("InGameColor")]
    [JsonConverter(typeof(StringEnumConverter))]
    public TeamColor InGameColor { get; set; }

    public string Castling { get; set; }

    public string EnPassant { get; set; }

    public int HalfMoveClock { get; set; }

    public int MoveNumber { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

}
