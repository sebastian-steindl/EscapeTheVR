### Coding Guidelines

Art | Schreibweise | Beispiel
--- | ------------ | --------
Einrückung | 4 Leerzeichen | -
Klassen | PascalCase | `public class MyClass`
Methoden, Funktionen | PascalCase | `public bool Hello()` 
Properties, public Member-Vars | PascalCase | `public float Distance = 0.0f;`
private Member-Vars | _camelCase | `private int _bob = 12;`
Funktions-Parameter | camelCase | `public void SetName(string elementName)`
Lokale Variablen | camelCase | `var half = whole / 2.0f;`
Konstanten | SNAKE_CASE | `const float HALF_PI = 1.570796;`
Kommentare | Englisch, Deutsch ok | -
Doku-Kommentare | Deutsch? | -

Beispiel:

```c#
public class Vector2D
{
    public float X;
    public float Y;
    
    // ---- alternativ X, Y als Property:
    private float _x;
    public float X
    {
        get { return _x; }
        set { _x = value; }
    }
    
    private float _y;
    public float Y
    {
        get { return _y; }
        set { _y = value; }
    }
    // ----
    
    public Vector2D(float x, float y)
    {
        X = x;
        Y = y;
    }
   
    public float Length()
    {
        var temp = X * X + Y * Y;
        return (float)Math.Sqrt(temp);
    }
    
    private bool UselessFunction()
    {
        return false;
    }
}
```
