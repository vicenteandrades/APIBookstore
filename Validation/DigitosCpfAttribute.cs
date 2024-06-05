using System.ComponentModel.DataAnnotations;

namespace APIBookstore.Validation
{
    public class DigitosCpfAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if(value is null)
            {
                return new ValidationResult("Objeto não pode ser nulo");
            }

            var cpf = value.ToString();

            if(cpf.Length != 14)
            {
                return new ValidationResult("Preencha um cpf válido");
            }

            return ValidationResult.Success;
        }

    }
}
