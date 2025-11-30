using System.Collections.Generic;
using FleetMaster.Core.Entities;

namespace FleetMaster.BusinessLogic.Validators
{
    public class OrderValidator
    {
        public List<string> Validate(Order order)
        {
            var validationErrors = new List<string>();

            if (order == null)
            {
                validationErrors.Add("Замовлення не може бути пустим.");
                return validationErrors;
            }

            if (order.WeightKg <= 0)
            {
                validationErrors.Add("Вага вантажу має бути більше 0 кг.");
            }

            if (order.WeightKg > 40000)
            {
                validationErrors.Add("Перевищено ліміт ваги (макс 40 тон). Потрібен спецдозвіл.");
            }

            if (order.Price < 0)
            {
                validationErrors.Add("Ціна замовлення не може бути від'ємною.");
            }

            if (string.IsNullOrWhiteSpace(order.Destination))
            {
                validationErrors.Add("Не вказано адресу доставки.");
            }
            else if (order.Destination.Length < 5)
            {
                validationErrors.Add("Адреса доставки занадто коротка. Уточніть дані.");
            }

            return validationErrors;
        }
    }
}