using System.ComponentModel.DataAnnotations;

namespace APIBookstore.Validation
{
    public class ValorMinimoEstoqueAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if(value is null)
            {
                return new ValidationResult("Não pode ser null.");
            }

            var num = Convert.ToInt32(value);

            if(num <= 0)
            {
                return new ValidationResult("Temos que ter pelo menos 1(um) produto no estoque");
            }

            return ValidationResult.Success;
        }


    }
}
