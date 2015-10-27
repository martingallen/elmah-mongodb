using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Collections.Specialized;

namespace Elmah
{
    public class NameValueCollectionSerializer : SerializerBase<NameValueCollection>
    {
        BsonDocumentSerializer documentserializer = new BsonDocumentSerializer();
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, NameValueCollection value)
        {
            BsonDocument doc = new BsonDocument();
            if (value != null)
            {
                foreach (var key in value.AllKeys)
                {
                    foreach (var val in value.GetValues(key))
                    {
                        doc.Add(key.Replace('.', '|'), val);
                    }
                }
            }

            documentserializer.Serialize(context, doc);
        }

        public override NameValueCollection Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            BsonDocument doc = documentserializer.Deserialize(context);
            var nvc = new NameValueCollection();
            foreach (var prop in doc)
            {
                nvc.Add(prop.Name.Replace('|', '.'), prop.Value.ToString());
            }

            return nvc;
        }
    }
}