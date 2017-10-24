    Создать три класса - базовый класс и два его потомка. Классы должны описывать описывающих сотрудников:

        Базовый сотрудник;

        Сотрудник с почасовой оплатой;

        Сотрудник с фиксированной оплатой;



    Классы должны иметь поле, в котором сохраняется ставка сотрудника:

        Для «повременщиков» - почасовая ставка;

        Для сотрудников с фиксированной з/п – месячная оплата труда;



    Описать в базовом классе абстрактный метод для расчета среднемесячной заработной платы. Переписать метод для потомков. Формула расчета з/п:

        Для повременной оплаты: «среднемесячная з/п = 20.8 * 8 * почасовую ставку»;

        Для с фиксированной оплаты: «среднемесячная з/п = фиксированной месячной оплате»;



    Создать REST сервис, предоставляющий следующие функции:

        Записать данные о сотруднике в БД;

        Получить данных сотрудника по его ФИО;

        Получить данные всех сотрудников с упорядочиванием по убыванию среднемесячного заработка. Вывести:

            Идентификатор сотрудника;

            ФИО;

            Среднемесячная з/п;

        Получить суммарную з/п всех сотрудников за месяц;

        Получить сотрудника с самой высокой почасовой ставкой;



    Реализовать REST клиент для описанного выше сервиса. Клиент должен вызывать методы разработанного сервиса. Результаты вызова выводить на консоль.



    При вызове метода «4.3. Получить данные всех сотрудников», клиент должен:

        При совпадении зарплаты – упорядочивать сотрудников с одинаковой з/п по фамилии в алфавитном порядке;

        Сохранять всех сотрудников с з/п выше средней в xml-файле предложенном вами формате;



    Разработку выполнить на C#.

    Клиент и сервер реализовать в виде консольного .Net Core приложения.

    Использовать любую реляционную БД: MS SQL Server, MySQL и т.д.

    Для манипуляций с множествами на стороне клиента использовать linq.

    При работе с БД не использовать ORM системы.

    Использовать Dependency Injection для связывания отдельных компонентов приложения.

    Исходный код проекта желательно опубликовать на сайте http://github.com и выслать ссылку на вашу реализацию.

    Приложить скрипт для развертывания БД или эталонную БД с данными.
