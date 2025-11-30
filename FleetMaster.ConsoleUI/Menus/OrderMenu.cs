using FleetMaster.BusinessLogic.Services;
using FleetMaster.Core.Entities;
using FleetMaster.ConsoleUI.Utilities;

namespace FleetMaster.ConsoleUI.Menus
{
    public class OrderMenu
    {
        private readonly OrderService _orderService;

        public OrderMenu(OrderService orderService)
        {
            _orderService = orderService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ЛОГІСТИКА ТА ЗАМОВЛЕННЯ ===");
                Console.WriteLine("1. Створити замовлення");
                Console.WriteLine("2. Автоматично призначити машину та водія");
                Console.WriteLine("0. Назад");

                int choice = InputHelper.ReadInt("Вибір", 0, 2);

                switch (choice)
                {
                    case 1: CreateOrder(); break;
                    case 2: AssignOrder(); break;
                    case 0: return;
                }
                Console.ReadKey();
            }
        }

        private void CreateOrder()
        {
            var order = new Order
            {
                Description = InputHelper.ReadString("Опис вантажу"),
                Destination = InputHelper.ReadString("Куди їдемо"),
                WeightKg = InputHelper.ReadDouble("Вага (кг)"),
                Price = InputHelper.ReadDouble("Ціна замовлення")
            };

            try
            {
                _orderService.CreateOrder(order);
                Console.WriteLine($"Замовлення #{order.Id} створено! Статус: {order.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        private void AssignOrder()
        {
            int orderId = InputHelper.ReadInt("ID Замовлення для розподілу", 1, 10000);

            Console.WriteLine("Спроба знайти машину та водія...");
            try
            {
                _orderService.AssignVehicleToOrder(orderId);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Успіх! Замовлення передано в роботу.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Невдача: {ex.Message}");
                Console.WriteLine("Створіть більше вільних машин або водіїв.");
                Console.ResetColor();
            }
        }
    }
}