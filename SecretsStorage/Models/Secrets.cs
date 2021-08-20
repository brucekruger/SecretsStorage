using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecretsStorage.Models
{
    public class Secrets : IValidatableObject
    {
 
        [Required]
        public string JwtKey { get; set; }

        [Required]
        public Mail Mail { get; set; }

        [Required]
        public string MongoDBConnectionString { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(Mail, new ValidationContext(Mail), validationResults, true);

            return validationResults;
        }
    }
}
