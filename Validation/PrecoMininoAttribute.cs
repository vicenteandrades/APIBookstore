using System.ComponentModel.DataAnnotations;

namespace APIBookstore.Validation
{
    public class PrecoMininoAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if(value is null)
            {
                return new ValidationResult("Objeto não pode ser nulo.");
            }

            double preco = Convert.ToDouble(value);

            if(preco <= 0)
            {
                return new ValidationResult("O produto não pode custar R$0, sinto muito.");
            }

            return ValidationResult.Success;

        }

    }
}
