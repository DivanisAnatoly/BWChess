using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ChessLibrary
{
    class ForsythEdwardsNotation
    {
        public string PiecePosition { get; set; }

        [JsonProperty("InGameColor")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Color InGameColor { get; set; }

        public string Castling { get; set; }

        public string EnPassant { get; set; }

        public int HalfMoveClock { get; set; }

        public int MoveNumber { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
