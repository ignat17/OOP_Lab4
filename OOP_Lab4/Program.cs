using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Введiть максимальну кiлькiсть продуктiв: ");
        string maxInput = Console.ReadLine();

        if (!int.TryParse(maxInput, out int maxCount) || maxCount <= 0)
            throw new Exception("Кiлькiсть повинна бути бiльшою за нуль.");

        List<Product> products = new List<Product>();

        while (true)
        {
            Console.WriteLine("\nМЕНЮ:");
            Console.WriteLine("1 - Додати продукт");
            Console.WriteLine("2 - Переглянути всi продукти");
            Console.WriteLine("3 - Знайти продукт");
            Console.WriteLine("4 - Продемонструвати поведiнку");
            Console.WriteLine("5 - Видалити продукт");
            Console.WriteLine("6 - Продемонструвати static-методи");
            Console.WriteLine("0 - Вийти");
            Console.Write("Ваш вибiр: ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": addproduct(products, maxCount); break;
                    case "2": viewall(products); break;
                    case "3": findproduct(products); break;
                    case "4": demonstrbehav(products); break;
                    case "5": deleteproduct(products); break;
                    case "6": demonstratestatic(); break;
                    case "0": return;
                    default: throw new Exception("Невiрний пункт меню.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
    }

    static void addproduct(List<Product> list, int max)
    {
        if (list.Count >= max)
            throw new Exception("Досягнуто максимальної кiлькостi продуктiв.");

        Console.WriteLine("\nОберiть спосiб додавання продукту:");
        Console.WriteLine("1 - Через конструктор");
        Console.WriteLine("2 - Через рядок (TryParse)");
        Console.Write("Ваш вибiр: ");

        string mode = Console.ReadLine();

        if (mode == "1")
        {
            addByConstructors(list, max);
        }
        else if (mode == "2")
        {
            Console.WriteLine("Введiть рядок формату: Name, Category, Price, Stock");
            string s = Console.ReadLine();

            if (Product.TryParse(s, out Product p))
            {
                list.Add(p);
                Console.WriteLine("Продукт успiшно створено методом TryParse.");
            }
            else
            {
                throw new Exception("Рядок має некоректний формат.");
            }
        }
        else
        {
            throw new Exception("Невiрний вибiр.");
        }
    }
    static void addByConstructors(List<Product> list, int max)
    {
        Console.WriteLine("\nОберiть конструктор:");
        Console.WriteLine("1 - Конструктор без параметрiв");
        Console.WriteLine("2 - Конструктор (Name, Price)");
        Console.WriteLine("3 - Конструктор (Name, Category, Price, Stock)");
        Console.Write("Ваш вибiр: ");

        string mode = Console.ReadLine();
        Product p;

        if (mode == "1")
        {
            p = new Product();
            Console.WriteLine("Створено продукт конструктором №1.");
        }
        else if (mode == "2")
        {
            string name = validname();
            decimal price = validprice();
            p = new Product(name, price);
            Console.WriteLine("Створено продукт конструктором №2.");
        }
        else if (mode == "3")
        {
            string name = validname();
            Category cat = validcategory();
            decimal price = validprice();
            int stock = validstock();
            p = new Product(name, cat, price, stock);
            Console.WriteLine("Створено продукт конструктором №3.");
        }
        else
            throw new Exception("Невiрний вибiр конструктора.");

        list.Add(p);
        Console.WriteLine("Продукт додано.");
    }


    static string validname()
    {
        Console.Write("Введiть назву продукту: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || input.Length < 3 || input.Length > 30)
            throw new Exception("Назва повинна мати 3–30 символiв.");

        return input;
    }

    static decimal validprice()
    {
        Console.Write("Введiть цiну: ");
        string input = Console.ReadLine();
        input = input.Replace('.', ',');

        if (!decimal.TryParse(input, out decimal price))
            throw new Exception("Цiна повинна бути числом.");

        if (price < 0.01m || price > 100000m)
            throw new Exception("Цiна повинна бути в межах 0.01–100000.");

        return price;
    }

    static int validstock()
    {
        Console.Write("Введiть кiлькiсть на складi: ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int stock))
            throw new Exception("Кiлькiсть повинна бути числом.");

        if (stock < 0 || stock > 10000)
            throw new Exception("Кiлькiсть повинна бути в межах 0–10000.");

        return stock;
    }

    static Category validcategory()
    {
        Console.Write("Введiть категорiю (Electronics, Clothes, Food, Furniture, Cosmetics): ");
        string input = Console.ReadLine();

        if (!Enum.TryParse(typeof(Category), input, true, out object result))
            throw new Exception("Такої категорiї не iснує.");

        return (Category)result;
    }


    static void viewall(List<Product> list)
    {
        if (list.Count == 0)
            throw new Exception("Список продуктiв порожнiй.");

        Console.WriteLine("\nЛiчильник створених об’єктiв: " + Product.Count);

        Console.WriteLine("\n#   | Назва                | Категорiя     |   Цiна    | Кiлькiсть");
        Console.WriteLine("-----------------------------------------------------------------------");

        int i = 1;
        foreach (var p in list)
        {
            Console.WriteLine($"{i,-3} | {p.Name,-20} | {p.Category,-12} | {p.Price,9:F2} | {p.Stock,8}");
            i++;
        }
    }

    static void findproduct(List<Product> list)
    {
        if (list.Count == 0)
            throw new Exception("Немає продуктiв для пошуку.");

        Console.WriteLine("1 - Назва");
        Console.WriteLine("2 - Категорiя");
        Console.Write("Ваш вибiр: ");

        string choice = Console.ReadLine();
        List<Product> results = new List<Product>();

        if (choice == "1")
        {
            Console.Write("Введiть назву: ");
            string name = Console.ReadLine();
            foreach (var p in list)
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    results.Add(p);
        }
        else if (choice == "2")
        {
            Category cat = validcategory();
            foreach (var p in list)
                if (p.Category == cat)
                    results.Add(p);
        }
        else throw new Exception("Невiрний вибiр.");

        if (results.Count == 0)
            Console.WriteLine("Не знайдено.");
        else
        {
            Console.WriteLine("\nРезультати:");
            foreach (var p in results)
                Console.WriteLine($"{p.Name} | {p.Category} | {p.Price} | {p.Stock}");
        }
    }


    static void demonstrbehav(List<Product> list)
    {
        if (list.Count == 0)
            throw new Exception("Немає продуктiв для демонстрацiї.");

        viewall(list);
        Console.Write("Оберiть продукт за номером: ");

        if (!int.TryParse(Console.ReadLine(), out int num) || num < 1 || num > list.Count)
            throw new Exception("Некоректний номер.");

        Product p = list[num - 1];

        Console.WriteLine("\nОберiть дiю:");
        Console.WriteLine("1 - Поповнити склад");
        Console.WriteLine("2 - Придбати товар");
        Console.WriteLine("3 - Знижка (звичайна)");
        Console.WriteLine("4 - Знижка з вiдсотком");
        Console.WriteLine("5 - Примусова знижка");
        Console.Write("Ваш вибiр: ");

        string ch = Console.ReadLine();

        switch (ch)
        {
            case "1":
                Console.Write("Кількiсть: ");
                p.restock(int.Parse(Console.ReadLine()));
                break;

            case "2":
                Console.Write("Кількiсть: ");
                p.purchase(int.Parse(Console.ReadLine()));
                break;

            case "3":
                p.discount();
                break;

            case "4":
                Console.Write("Вiдсоток: ");
                p.discount(double.Parse(Console.ReadLine()));
                break;

            case "5":
                Console.Write("Вiдсоток: ");
                p.discount(double.Parse(Console.ReadLine()), true);
                break;

            default:
                throw new Exception("Невiрний вибiр.");
        }

        Console.WriteLine("\nОновлено:");
        Console.WriteLine(p.ToString());
    }

    static void deleteproduct(List<Product> list)
    {
        if (list.Count == 0)
            throw new Exception("Немає продуктiв для видалення.");

        Console.WriteLine("1 - За номером");
        Console.WriteLine("2 - За категорiєю");
        Console.Write("Ваш вибiр: ");

        string ch = Console.ReadLine();

        if (ch == "1")
        {
            viewall(list);
            Console.Write("Номер: ");
            int num = int.Parse(Console.ReadLine());
            if (num < 1 || num > list.Count)
                throw new Exception("Некоректний номер.");
            list.RemoveAt(num - 1);
            Console.WriteLine("Видалено.");
        }
        else if (ch == "2")
        {
            Category cat = validcategory();
            int removed = list.RemoveAll(p => p.Category == cat);
            Console.WriteLine($"Видалено: {removed}");
        }
        else
            throw new Exception("Невiрний вибiр.");
    }


    static void demonstratestatic()
    {
        Console.WriteLine("\n--- Демонстрацiя static-методiв ---\n");

        Console.WriteLine("1) Вивести обмеження продукту (getRestrictions):");
        Console.WriteLine(Product.getRestrictions());

        Console.WriteLine("2) Поточний формат за замовчуванням (defaultcategory):");
        Console.WriteLine("DefaultCategory = " + Product.defaultcategory);

        Console.WriteLine("3) Демонстрацiя Parse:");
        Console.WriteLine("Спроба розiбрати рядок 'milk, Food, 25, 10'");

        Product p = Product.Parse("milk, Food, 25, 10");
        Console.WriteLine("Отримано продукт: " + p.ToString());

        Console.WriteLine("4) Демонстрацiя TryParse:");
        bool ok = Product.TryParse("Bad Input", out Product p2);
        Console.WriteLine("TryParse повернув: " + ok);
    }
}
