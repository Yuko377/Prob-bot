# Prob-bot
Бот для быстрых вычислений в теории вероятностей

## Участники:
- Тарасенко Александр, группа 19144
- Патрин Георгий, группа 19144

## Проблема:
Часто на парах или в процессе решения задач по теории вероятностей 
возникает потребность в вычислении специфических величин, которые 
на простом калькуляторе не посчитаешь. Наш бот позволит вычислять 
характиристики распределений прямо в мессенджере, не прибегая к 
программированию и специальным сервисам.

## Потенциальные пользователи:
- Студенты
- преподаватели теории вероятностей на парах
- все, кто решает задачи на теорию вероятностей

## Основные сценарии использования:
1. чтобы начать новую работу с ботом, пользователь вводит "/start"
2. пользователь выбирает из предложенных распределений то, которое его интересует
3. задаёт ему параметры(вводит числа)
4. выбирает из предложенных характиристик то, что хочет посчитать
   - случайная величина
   - значение плотности в заданной точке
   - значение функции распределения в заданной точке
   - матожидание
   - дисперсия

на каждом шаге пользователь может вернуться на предыдущий

## Описание основных компонент системы:
- У каждого пользователя есть список состояний, в которых он находился 
в текущем сеанса
- Соответственно текущему состоянию он получает предлагаемые действия
(например состояние: выбирает распределение
предлагаемые действия: выбирете распределение из предложенных)
- Все распределения хранятся в виде отдельных txt файлах-классах в специальной папке,
откуда могут подгружаться без перезапуска системы
- Функциональность каждого класса описывается соответствующим набором интерфейсов

## Описание точек расширения:
- добавить новое распределение можно просто положив .txt файл с его классом 
в общую папку и перекомпилировав
- разделение на уровни позволит легко переподключить бота на другую платформу
- можно расширять фукнциональность бота, добавлением новых методов в классе распределения
чтобы новые методы начали работать, достаточно пометить их соответствующим атрибутом-названием