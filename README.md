# Лабораторна робота №4  
## Тема: Статичні поля, властивості, методи

### Виконав:
- **Студент:** Кранго Ігнат Андрійович  
- **Група:** 622п  
- **Освітня програма:** 121 Інженерія програмного забезпечення  

### Прийняв:
- **Доц. Лучшев П.О.**

---

## Мета роботи
- навчитися створювати й застосовувати статичні поля, властивості та методи;  
- реалізувати перетворення рядка у об’єкт класу через Parse і TryParse;  
- модифікувати програму з Lab-3 відповідно до принципів статичності в ООП.

---

## Опис класу `Product`

У лабораторній роботі №4 клас **Product**, створений у Lab-2 і розширений у Lab-3, було доповнено статичними елементами згідно вимог роботи.

### Основні дані:
- Назва продукту (`string`)
- Категорія продукту (`enum Category`)
- Ціна (`decimal`)
- Кількість на складі (`int`)

### Валідація:
- Назва повинна містити 3–30 символів;  
- Категорія — лише значення enum;  
- Ціна — у межах 0.01–100000;  
- Кількість — у межах 0–10000;  
- Некоректні значення спричиняють `Exception`.

---

## Поведінка класу:
- **restock(amount)** – поповнення товару;  
- **purchase(amount)** – придбання з перевіркою;  
- **discount(...)** – три перевантажені версії методу для знижок.  

---

## Додано для Lab-4:

### ✔ Статичні поля:
1. **_count** — лічильник створених об’єктів (збільшується у кожному конструкторі);  
2. **_defaultCategory** — характеристика предметної області (категорія за замовчуванням).

---

### ✔ Статичні властивості:

public static int Count               
public static string DefaultCategory  


## Програмна реалізація класу

```csharp
public class Product
{
    private string _name;
    private Category _category;
    private decimal _price;
    private int _stock;
    private DateTime _lastUpdated;

    private static int _count = 0;                         
    private static string _defaultCategory = "Food";   
    public static int Count => _count;
    public static string defaultcategory => _defaultCategory;

    public Product()
    {
        Name = "default";
        Category = Category.Food;
        Price = 10;
        Stock = 1;
        _count++;                                       
    }

    public Product(string name, decimal price) : this()
    {
        Name = name;
        Price = price;
    }

    public Product(string name, Category category, decimal price, int stock)
        : this(name, price)
    {
        Category = category;
        Stock = stock;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 30)
                throw new Exception("Назва повинна мати 3–30 символiв.");
            _name = value;
            update();
        }
    }

    public Category Category
    {
        get => _category;
        set
        {
            _category = value;
            update();
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0.01m || value > 100000m)
                throw new Exception("Цiна повинна бути в межах 0.01–100000.");
            _price = value;
            update();
        }
    }

    public int Stock
    {
        get => _stock;
        set
        {
            if (value < 0 || value > 10000)
                throw new Exception("Кiлькiсть повинна бути в межах 0–10000.");
            _stock = value;
            update();
        }
    }

    public int Id { get; set; } = 1;

    public string Summary =>
        $"{Name} | Категорiя: {Category} | Цiна: {Price} | Кiлькiсть: {Stock}";

    private void update()
    {
        _lastUpdated = DateTime.Now;
    }

    public void restock(int amount)
    {
        if (amount <= 0)
            throw new Exception("Кiлькiсть повинна бути бiльшою за 0.");
        Stock += amount;
    }

    public void purchase(int amount)
    {
        if (amount <= 0)
            throw new Exception("Кiлькiсть повинна бути бiльшою за 0.");
        if (amount > Stock)
            throw new Exception("Недостатньо товару на складi.");
        Stock -= amount;
    }

    public void discount()
    {
        Price *= 0.9m;
    }

    public void discount(double percent)
    {
        if (percent < 0 || percent > 90)
            throw new Exception("Знижка повинна бути 0–90%.");
        Price -= Price * (decimal)(percent / 100);
    }

    public void discount(double percent, bool force)
    {
        if (!force && (percent < 0 || percent > 90))
            throw new Exception("Знижка повинна бути 0–90%.");
        _price -= _price * (decimal)(percent / 100);
        update();
    }

    public static string getRestrictions()
    {
        return
            "Обмеження продукту:\n" +
            "- Назва: 3–30 символiв\n" +
            "- Категорiя: значення enum Category\n" +
            "- Цiна: 0.01–100000\n" +
            "- Кiлькiсть: 0–10000\n";
    }

    public override string ToString()
    {
        return $"{Name}, {Category}, {Price}, {Stock}";
    }

    public static Product Parse(string s)
    {
        if (string.IsNullOrEmpty(s))
            throw new ArgumentNullException("Рядок порожнiй.");

        var parts = s.Split(", ");

        if (parts.Length != 4)
            throw new ArgumentException("Невiрна кiлькiсть параметрiв. Формат: Name, Category, Price, Stock");

        string name = parts[0];

        if (!Enum.TryParse(typeof(Category), parts[1], true, out object cat))
            throw new Exception("Некоректна категорiя у рядку.");

        if (!decimal.TryParse(parts[2], out decimal price))
            throw new Exception("Некоректна цiна.");

        if (!int.TryParse(parts[3], out int stock))
            throw new Exception("Некоректна кiлькiсть.");

        return new Product(name, (Category)cat, price, stock);
    }

    public static bool TryParse(string s, out Product product)
    {
        try
        {
            product = Parse(s);
            return true;
        }
        catch
        {
            product = null;
            return false;
        }
    }
}
