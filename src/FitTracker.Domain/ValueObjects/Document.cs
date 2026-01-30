using FitTracker.Domain.Enums;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Primitives;
using FitTracker.Domain.Shared;
using System.Text.RegularExpressions;

namespace FitTracker.Domain.ValueObjects
{
    public class Document : ValueObject
    {
        public const int MaxLength = 14;

        public DocumentType Type { get; private set; }

        public string Value { get; private set; }

        private Document(string value, DocumentType type) 
        {
            Value = value;
            Type = type; 
        }

        private Document()
        {
        }

        public static Result<Document> Create(string document)
        {           
            string documentFormated = GetOnlyNumbers(document);

            DocumentType type = GetDocumentType(documentFormated);

            return Result.Create(documentFormated)
                .Ensure(
                    e => !string.IsNullOrWhiteSpace(e),
                    DomainErrors.Document.Empty)
                .Ensure(
                    e => type != DocumentType.Invalid,
                    DomainErrors.Document.InvalidFormat)
                .Map(e => new Document(e, type));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        private static DocumentType GetDocumentType(string document)
        {
            DocumentType type = DocumentType.Invalid;

            if (document.Length == 11)
            {
                type = DocumentType.CPF;
            }
            else if (document.Length == 14)
            {
                type = DocumentType.CNPJ;
            }

            return type;
        }

        private static string GetOnlyNumbers(string document)
        {
            return Regex.Replace(document, @"[^\d]", ""); ;
        }
    }
}
