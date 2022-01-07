using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Cropper {
    public class CroppedImageJsonConverter : JsonConverter {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) {
            return objectType == typeof(CroppedImage);
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer) {
            throw new NotImplementedException();
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value != null) {
                var croppedImage = (CroppedImage) value;

                if (croppedImage.IsSingle()) {
                    serializer.Serialize(writer, croppedImage.Crop);
                } else {
                    writer.WriteStartArray();

                    foreach (var crop in croppedImage) {
                        serializer.Serialize(writer, crop);
                    }
                    
                    writer.WriteEndArray();
                }
            }
        }
    }
}