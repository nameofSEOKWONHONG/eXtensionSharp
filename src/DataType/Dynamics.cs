using System.Collections;

namespace eXtensionSharp;

public sealed class HList<T1, T2, T3> : IReadOnlyList<Variant3<T1, T2, T3>>
{
    private readonly List<Variant3<T1, T2, T3>> _items = new();

    // 추가 오버로드
    public void Add(T1 v) => _items.Add(Variant3<T1, T2, T3>.From(v));
    public void Add(T2 v) => _items.Add(Variant3<T1, T2, T3>.From(v));
    public void Add(T3 v) => _items.Add(Variant3<T1, T2, T3>.From(v));

    // 인덱서/카운트/열거
    public Variant3<T1, T2, T3> this[int index] => _items[index];
    public int Count => _items.Count;
    public IEnumerator<Variant3<T1, T2, T3>> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // 타입별 처리/투영 헬퍼
    public void ForEach(System.Action<T1> a1, System.Action<T2> a2, System.Action<T3> a3)
    {
        foreach (var it in _items) it.Switch(a1, a2, a3);
    }

    public IEnumerable<TResult> Select<TResult>(
        System.Func<T1, TResult> f1, System.Func<T2, TResult> f2, System.Func<T3, TResult> f3)
    {
        foreach (var it in _items) yield return it.Match(f1, f2, f3);
    }

    // 특정 인덱스에서 원하는 타입만 꺼내기
    public bool TryGetAt<T>(int index, out T value)
    {
        var it = _items[index];
        if (typeof(T) == typeof(T1) && it.TryGet(out T1 v1)) { value = (T)(object)v1!; return true; }
        if (typeof(T) == typeof(T2) && it.TryGet(out T2 v2)) { value = (T)(object)v2!; return true; }
        if (typeof(T) == typeof(T3) && it.TryGet(out T3 v3)) { value = (T)(object)v3!; return true; }
        value = default!;
        return false;
    }
}

public readonly struct Variant3<T1, T2, T3>
{
    private readonly byte _tag; // 1: T1, 2: T2, 3: T3
    private readonly T1 _v1;
    private readonly T2 _v2;
    private readonly T3 _v3;

    private Variant3(byte tag, T1 v1, T2 v2, T3 v3)
    {
        _tag = tag;
        _v1 = v1;
        _v2 = v2;
        _v3 = v3;
    }

    public static Variant3<T1, T2, T3> From(T1 value) => new(1, value, default!, default!);
    public static Variant3<T1, T2, T3> From(T2 value) => new(2, default!, value, default!);
    public static Variant3<T1, T2, T3> From(T3 value) => new(3, default!, default!, value);

    public bool IsT1 => _tag == 1;
    public bool IsT2 => _tag == 2;
    public bool IsT3 => _tag == 3;

    public T1 AsT1 => IsT1 ? _v1 : throw new InvalidOperationException();
    public T2 AsT2 => IsT2 ? _v2 : throw new InvalidOperationException();
    public T3 AsT3 => IsT3 ? _v3 : throw new InvalidOperationException();

    public bool TryGet(out T1 value) { value = IsT1 ? _v1 : default!; return IsT1; }
    public bool TryGet(out T2 value) { value = IsT2 ? _v2 : default!; return IsT2; }
    public bool TryGet(out T3 value) { value = IsT3 ? _v3 : default!; return IsT3; }

    public TResult Match<TResult>(Func<T1, TResult> f1, Func<T2, TResult> f2, Func<T3, TResult> f3)
        => _tag switch
        {
            1 => f1(_v1),
            2 => f2(_v2),
            3 => f3(_v3),
            _ => throw new InvalidOperationException()
        };

    public void Switch(Action<T1> a1, Action<T2> a2, Action<T3> a3)
    {
        switch (_tag)
        {
            case 1: a1(_v1); break;
            case 2: a2(_v2); break;
            case 3: a3(_v3); break;
            default: throw new InvalidOperationException();
        }
    }
}