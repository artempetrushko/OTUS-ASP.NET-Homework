using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pcf.Administration.Core.Domain
{
    public class BaseEntity
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
    }
}