using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace SquirrelsNest.Pecan.Server.Support {
    public class DateOnlyConverter : JsonConverter<DateOnly> {
        private readonly string mSerializationFormat;

        public DateOnlyConverter() : this( String.Empty ) { }

        public DateOnlyConverter( string serializationFormat ) {
            mSerializationFormat = String.IsNullOrWhiteSpace( serializationFormat ) ? 
                serializationFormat : 
                "yyyy-MM-dd";
        }

        public override DateOnly Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            var value = reader.GetString();

            return DateOnly.Parse( value! );
        }

        public override void Write( Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options ) => 
            writer.WriteStringValue( value.ToString( mSerializationFormat ) );
    }
}
