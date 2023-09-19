



Console.WriteLine(new BracketSequence("(((){}[]]]])(").isCorrectSequence); // false
Console.WriteLine(new BracketSequence("(){}[][][]()").isCorrectSequence); // true
Console.WriteLine(new BracketSequence("({[]})()[]").isCorrectSequence); // true
Console.WriteLine(new BracketSequence("}{").isCorrectSequence); // false
Console.WriteLine(new BracketSequence("]{{(()})}[").isCorrectSequence); // false
Console.WriteLine(new BracketSequence("()()({}[])").isCorrectSequence); // true
Console.WriteLine(new BracketSequence("()()({}[])(").isCorrectSequence); // false
Console.WriteLine(new BracketSequence("()").isCorrectSequence); // true




class BracketSequence {
    static private Dictionary<char, char> _bracketType = new Dictionary<char, char>() {
            { '[', ']' },
            { '(', ')' },
            { '{', '}' }
        };

    private bool _isCorrect = true;
    public bool isCorrectSequence { get { return _isCorrect; } }
    public string Sequence { get; init; }
    public BracketSequence(string sequence) {
        Sequence = sequence;
        IsCorrectSequence(0, Sequence.Length - 1);
    }

    private void IsCorrectSequence(int i, int j) {
        if (i > j) return;
        if (_bracketType.Values.Contains(Sequence[i])) {
            _isCorrect = false;
            return;
        }
        if (_bracketType.Keys.Contains(Sequence[j])) {
            _isCorrect = false;
            return;
        }

        for (int k = i + 1, count = 1; k <= j; k++) {
            if (Sequence[k] == _bracketType[Sequence[i]]) {
                count--;
            } 
            if (Sequence[k] == Sequence[i]) { 
                count++;
            }
            if (count == 0) {
                if (i + 1 < k) {
                    IsCorrectSequence(i + 1, k - 1);
                }
                if (k < j - 1) {
                    IsCorrectSequence(k + 1, j);
                }
                return;
            }            
        }
        _isCorrect = false;
    }
}
