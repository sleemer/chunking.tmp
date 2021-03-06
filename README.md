# Тестовое Задание:

### Составить частотный словарь для заданного текстового файла.

Программа должна быть реализована в виде консольного приложения. Первым параметром задается имя входного файла. Вторым параметром задается имя файла с результатом.
Кодировкой исходного и результирующего файлов считается Windows-1251.
Слова разделяются пробелами и символами перевода строки. Слова сравниваются без учета регистра.
Формат результирующего файла – «{0},{1}\n» где {0} – слово, а {1} – количество его вхождений в исходный файл.
Результирующий файл должен быть отсортирован по убыванию встречаемости.

### Приложение должно обладать следующими качествами:
1) Производительностью, разумной экономностью ресурсов CPU и Memory.
2) Скалируемостью (от scalability) на многоядерных/многопроцессорных системах.
3) Использовать классы стандартной библиотеки.Net 3.5SP1 или 4.0.
4) Ожидать неправильного ввода от пользователя.
5) Иметь модульную декомпозицию и содержать юнит тесты.
6) Обладать архитектурой удобной для дальнейшего расширения функциональности.
7) Иметь описание архитектуры в любом удобном виде.

## Архитектура решения:
Для удовлетворения 1, 2, 5 и 6 требований был выбран дизайн системы основанный на разбиении исходного файла на chunk'и, паралельной независимой друг-от-друга обработки каждого chunk'а и в конечном итоге merge результатов обработки индивидуальных chunk'ов в один общий словарь. 

В качестве механизма синхронизации между чтением chunk'ов из файла и их обработкой был выбран Producer-Consumer pattern и одна из его доступных реализаций в .net framework 4.0 (требование 3) - BlockingCollection<T>. Использование данной коллекции позволяет распаралеллить обработку chunk'ов в разных потоках, а также предоставляет механизм контроля максимально используемой памяти, что позволит избежать проблем с "очень быстрым чтением и медленной обработкой". 

В текущем решении используется один Producer и несколько Consumer'ов которые запускаются через Parallel.For (который хорошо подходит для решения задач с data parallelism). Можно было бы использовать и PLINQ, но Parallel более дружественен к шарингу ресурсов между процессами в системе.

Для удовлетворения требования о том, что частотный словарь должен быть отсортирован по убыванию встречаемости, я использовал стандартные механизмы Linq, т.к. в текущей задаче узким горлышком является механизм составления словаря, а его сортировка может быть выполнена стандартными средствами .net с приемлемой скоростью.
 
### Алгоритм составления частотного словаря файла имеет модульную структуру и представлен следующими основными интерфейсами.
- IWordFrequencyCounter это основной интерфейс алгоритма, который позволяет вызывающему коду абстрагироваться от деталей имплементации алгоритма, в данном случае от алгоритма разбиения на chunk'и, вычисления частотных характеристик для chunk'ов и сведения (merge) частотного словаря для всего файла.
- IChunkFileReader - интерфейс алгоритма чтения исходного файла и разбивки его на chunk'и.
    - Я реализовал два разных механизма. Первый - простой способ построчного чтения, второй основан на блочном чтении из файла цельных блоков с заданным максимальным размером. Второй способ также позволяет более оптимально работать с файлами, где сильно варируется длина строк, либо весь файл представлен одной строкой.
- IChunkProcessor - интерфейс составления частотного словаря для группы chunk'ов
- IChunkResultMerger - интерфейс алгоритма склейки частотных словарей отдельных chunk'ов. 

### В качестве дальнейшей оптимизации алгоритма можно рассмотреть:
- использование массивов из пулов, чтобы избежать большого количества аллокаций временных массивов и строк на куче и в LOH и как следствие частых GC 1 и второго покаления. Как можно видеть из приведенных результатов прогона perf-тестов текущие реализации алгоритма страдают прожорливостью по сравнению с baseline-алгоритмом, хоть и в два раза быстрее в тестах.
- переход на использование структуры данных Trie (https://en.wikipedia.org/wiki/Trie) и использовать ее вместо Dictionary<string, int>. Это должно позволить снизить нагрузку на память, т.к. можно будет отказаться от разбиения chunk'а на слова, а двигаясь по chunk'у слева направо находить(вставлять) нужную node'у и хранить частоту встречаемости слова в одном из полей node'ы.
- далее, если позволят отказаться от ограничений первоначальной задачи на .net framework 4.0, то можно будет перейти на использование .net core и его новых классов Span<T> и Memory<T>, а также новой библиотеки System.IO.Pipelines которые как раз заточены под использование в сценариях с парсингом строк.

### Результаты прогона perf-теста на 50Мб файле

                                Method |       Mean |     Error |    StdDev | Scaled |       Gen 0 |       Gen 1 |      Gen 2 |  Allocated |
-------------------------------------- |-----------:|----------:|----------:|-------:|------------:|------------:|-----------:|-----------:|
        SimpleWordFrequencyCounterTest | 1,951.4 ms | 20.339 ms | 18.030 ms |   1.00 | 322812.5000 |    187.5000 |          - |  968.89 MB |
         ChunkWordFrequencyCounterTest | 1,090.8 ms | 17.554 ms | 14.658 ms |   0.56 | 342125.0000 |   9000.0000 |   687.5000 | 1036.67 MB |
 BufferedChunkWordFrequencyCounterTest |   808.5 ms |  4.899 ms |  4.343 ms |   0.41 | 308125.0000 | 138437.5000 | 26187.5000 | 1207.02 MB |