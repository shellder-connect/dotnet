
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Models;

    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? IdTipoUsuario { get; set; }
        public string? Telefone { get; set; }
        public string? IdEndereco { get; set; }
        public string? DataNascimento { get; set; }
        public string? Documento { get; set; }
        public string? Status { get; set; }
    }