namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public class SyntaxStrings : ISyntaxStrings, IStringList, ICollection, IEnumerable, ITextSearch, IExport, IImport, ITabulationEx, ITabulation, IWordBreak, INotifyEx, INotify
    {
        // Events
        private event EventHandler notifyHandler;

        // Methods
        public SyntaxStrings()
        {
            this.currentIndex = 0;
            int[] numArray1 = new int[1] { EditConsts.DefaultTabStop } ;
            this.tabStops = numArray1;
            char[] chArray1 = new char[1] { '\t' } ;
            this.tabArray = chArray1;
            this.firstChanged = -1;
            this.lastChanged = -1;
            this.list = new ArrayList();
            this.tabList = new ArrayList();
            this.tabBuilder = new StringBuilder();
            this.delimTable = new Hashtable();
            this.InitDelimiters(EditConsts.DefaultDelimiters.ToCharArray());
        }

        public SyntaxStrings(TextSource Source) : this()
        {
            this.source = Source;
        }

        public int Add(string value)
        {
            int num1 = this.list.Add(this.CreateStrItem(value));
            this.Changed(num1);
            return num1;
        }

        public void AddNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Combine(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public void Assign(ISyntaxStrings Source)
        {
            this.BeginUpdate();
            try
            {
                this.Clear();
                foreach (string text1 in Source)
                {
                    this.Add(text1);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public int BeginUpdate()
        {
            if (this.updateCount == 0)
            {
                this.firstChanged = -1;
                this.lastChanged = -1;
            }
            this.updateCount++;
            return this.updateCount;
        }

        public void Changed(int Index)
        {
            this.Changed(Index, Index);
        }

        public void Changed(int First, int Last)
        {
            if (this.firstChanged == -1)
            {
                this.firstChanged = First;
            }
            else
            {
                this.firstChanged = Math.Min(this.firstChanged, First);
            }
            this.lastChanged = Math.Max(this.lastChanged, Last);
            this.StringsChanged();
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(string value)
        {
            foreach (StrItem item1 in this.list)
            {
                if (item1.String.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            this.list.CopyTo(array, index);
        }

        public virtual StrItem CreateStrItem(string S)
        {
            if (this.source != null)
            {
                return this.source.CreateStrItem(S);
            }
            return new StrItem(S);
        }

        public int EndUpdate()
        {
            this.updateCount--;
            this.StringsChanged();
            return this.updateCount;
        }

        ~SyntaxStrings()
        {
            this.delimTable.Clear();
        }

        public bool Find(string String, SearchOptions Options, Regex Expression, ref Point Position, out int Len)
        {
            return SyntaxStrings.Find(this, this.delimTable, String, Options, Expression, ref Position, out Len);
        }

        public static bool Find(IStringList List, Hashtable DelimTable, string String, SearchOptions Options, Regex Expression, ref Point Position, out int Len)
        {
            Len = String.Length;
            if ((String != null) && (String != string.Empty))
            {
                bool flag1 = ((Options & SearchOptions.CaseSensitive) == SearchOptions.None) && (Expression == null);
                if (flag1)
                {
                    String = String.ToUpper();
                }
                string text1 = null;
                int num1 = 0;
                int num2 = 0;
                if ((Options & SearchOptions.BackwardSearch) == SearchOptions.None)
                {
                Label_031C:
                    if (Position.Y < List.Count)
                    {
                        text1 = List[Position.Y];
                        if ((Position.X < text1.Length) || (((text1 == string.Empty) && (Position.X == 0)) && (Expression != null)))
                        {
                            if (flag1)
                            {
                                text1 = text1.ToUpper();
                            }
                            Position.X = Math.Min(Position.X, (int) (text1.Length - 1));
                            if (Expression != null)
                            {
                                Match match2 = (text1 == string.Empty) ? Expression.Match(text1) : Expression.Match(text1, Position.X);
                                if (match2.Success)
                                {
                                    Len = match2.Length;
                                    Position.X = match2.Index;
                                }
                                else
                                {
                                    Position.X = -1;
                                }
                            }
                            else if (text1 != string.Empty)
                            {
                                Position.X = text1.IndexOf(String, Position.X);
                            }
                            else
                            {
                                Position.X = -1;
                            }
                            if (((Position.X >= 0) && ((Options & SearchOptions.WholeWordsOnly) != SearchOptions.None)) && !SyntaxStrings.IsWholeWord(DelimTable, text1, Position.X, Len, ref num1, ref num2))
                            {
                                Position.X = (Position.X == num2) ? (Position.X + 1) : num2;
                                goto Label_031C;
                            }
                            if (Position.X >= 0)
                            {
                                return true;
                            }
                        }
                        Position.Y++;
                        Position.X = 0;
                        goto Label_031C;
                    }
                }
                else
                {
                    Position.Y = Math.Min(Position.Y, (int) (List.Count - 1));
                    while (Position.Y >= 0)
                    {
                        text1 = List[Position.Y];
                        if ((Position.X > 0) || (((text1 == string.Empty) && (Position.X == 0)) && (Expression != null)))
                        {
                            if (flag1)
                            {
                                text1 = text1.ToUpper();
                            }
                            if (Expression != null)
                            {
                                Match match1 = (text1 == string.Empty) ? Expression.Match(text1) : Expression.Match(text1, Math.Min(Position.X, (int) (text1.Length - 1)));
                                if (match1.Success)
                                {
                                    Len = match1.Length;
                                    Position.X = match1.Index;
                                }
                                else
                                {
                                    Position.X = -1;
                                }
                            }
                            else if (text1 != string.Empty)
                            {
                                Position.X = text1.LastIndexOf(String, Math.Min((int) (Position.X - 1), (int) (text1.Length - 1)));
                            }
                            else
                            {
                                Position.X = -1;
                            }
                            if (((Position.X >= 0) && ((Options & SearchOptions.WholeWordsOnly) != SearchOptions.None)) && !SyntaxStrings.IsWholeWord(DelimTable, text1, Position.X, Len, ref num1, ref num2))
                            {
                                Position.X = num1 - 1;
                                continue;
                            }
                            if (Position.X >= 0)
                            {
                                return true;
                            }
                        }
                        Position.Y--;
                        if (Position.Y >= 0)
                        {
                            Position.X = List[Position.Y].Length;
                        }
                    }
                }
            }
            return false;
        }

        public char GetCharAt(Point Position)
        {
            return this.GetCharAt(Position.X, Position.Y);
        }

        public char GetCharAt(int X, int Y)
        {
            StrItem item1 = this.GetItem(Y);
            if (((item1 != null) && (X >= 0)) && (X < item1.String.Length))
            {
                return item1.String[X];
            }
            return '\0';
        }

        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public string GetIndentString(int Count, int P)
        {
            return this.GetIndentString(Count, P, this.useSpaces);
        }

        protected internal string GetIndentString(int Count, int P, bool UseSpaces)
        {
            if (UseSpaces)
            {
                return new string(' ', Count);
            }
            int num1 = 0;
            int num2 = 0;
            while (num1 < Count)
            {
                int num3 = this.GetTabStop(P) - P;
                if ((num1 + num3) > Count)
                {
                    break;
                }
                num1 += num3;
                P += num3;
                num2++;
            }
            return (new string('\t', num2) + new string(' ', Count - num1));
        }

        public StrItem GetItem(int Index)
        {
            if ((Index >= 0) && (Index < this.Count))
            {
                return (StrItem) this.list[Index];
            }
            return this.CreateStrItem(string.Empty);
        }

        public int GetLength(int Index)
        {
            if ((Index >= 0) && (Index < this.Count))
            {
                return ((StrItem) this.list[Index]).String.Length;
            }
            return 0;
        }

        public int GetPrevTabStop(int Pos)
        {
            this.GetTabStop(Pos);
            int num1 = (int) this.tabList[Pos];
            while ((Pos > 0) && (((int) this.tabList[Pos]) == num1))
            {
                Pos--;
            }
            if (num1 < 0)
            {
                return 0;
            }
            return (int) this.tabList[Pos];
        }

        private int GetStop(int Pos)
        {
            int num1 = this.tabStops.Length;
            if (num1 == 0)
            {
                return EditConsts.DefaultTabStop;
            }
            int num2 = Pos - this.tabStops[num1 - 1];
            int num3 = 0;
            if (num2 >= 0)
            {
                if (num1 == 1)
                {
                    num3 = this.tabStops[0];
                }
                else
                {
                    num3 = this.tabStops[num1 - 1] - this.tabStops[num1 - 2];
                }
                return (this.tabStops[num1 - 1] + (((num2 / num3) + 1) * num3));
            }
            num3 = 0;
            while (Pos >= this.tabStops[num3])
            {
                num3++;
            }
            return this.tabStops[num3];
        }

        public int GetTabStop(int Pos)
        {
            if (Pos == 0x7fffffff)
            {
                return Pos;
            }
            for (int num1 = this.tabList.Count; num1 <= Pos; num1++)
            {
                this.tabList.Add(this.GetStop(num1));
            }
            return (int) this.tabList[Pos];
        }

        public string GetTabString(string s)
        {
            short[] numArray1 = null;
            this.GetTabString(ref s, ref numArray1, false);
            return s;
        }

        protected internal void GetTabString(ref string String, ref short[] Data, bool NeedData)
        {
            if (((String != null) && (String != string.Empty)) && (String.IndexOf('\t') >= 0))
            {
                this.tabBuilder.Length = 0;
                int num1 = 0;
                int num2 = 0;
                bool flag1 = true;
                string[] textArray1 = String.Split(this.tabArray);
                string[] textArray2 = textArray1;
                int num6 = 0;
                while (num6 < textArray2.Length)
                {
                    string text1 = textArray2[num6];
                    if (flag1)
                    {
                        this.tabBuilder.Append(text1);
                        flag1 = false;
                        num1 = text1.Length;
                    }
                    else
                    {
                        num2 = this.GetTabStop(num1) - num1;
                        this.tabBuilder.Append(new string(' ', num2));
                        this.tabBuilder.Append(text1);
                        num1 += (text1.Length + num2);
                    }
                    num6++;
                }
                String = this.tabBuilder.ToString();
                if (NeedData)
                {
                    short[] numArray1 = new short[String.Length];
                    num1 = 0;
                    int num3 = 0;
                    num2 = 0;
                    flag1 = true;
                    textArray2 = textArray1;
                    for (num6 = 0; num6 < textArray2.Length; num6++)
                    {
                        string text2 = textArray2[num6];
                        if (flag1)
                        {
                            flag1 = false;
                            num1 = text2.Length;
                            num3 = num1;
                            if (num1 != 0)
                            {
                                Array.Copy(Data, 0, numArray1, 0, num1);
                            }
                        }
                        else
                        {
                            num2 = this.GetTabStop(num1) - num1;
                            for (int num5 = num1; num5 < (num1 + num2); num5++)
                            {
                                numArray1[num5] = Data[num3];
                            }
                            StrItem.SetColorFlag(ref numArray1, num1 + (num2 / 2), 1, 2, true);
                            int num4 = text2.Length;
                            Array.Copy(Data, (int) (num3 + 1), numArray1, (int) (num1 + num2), num4);
                            num1 += (num4 + num2);
                            num3 += (num4 + 1);
                        }
                    }
                    Data = numArray1;
                }
            }
        }

        public string GetTextAt(Point Position)
        {
            return this.GetTextAt(Position.X, Position.Y);
        }

        public string GetTextAt(int Pos, int Line)
        {
            int num1;
            int num2;
            string text1 = this[Line];
            if (this.GetWord(text1, Pos, out num1, out num2))
            {
                return text1.Substring(num1, (num2 - num1) + 1);
            }
            return string.Empty;
        }

        public bool GetWord(int Index, int Pos, out int Left, out int Right)
        {
            return this.GetWord(this[Index], Pos, out Left, out Right);
        }

        public bool GetWord(string String, int Pos, out int Left, out int Right)
        {
            int num1 = String.Length;
            Left = 0;
            Right = 0;
            if ((String == string.Empty) || (Pos > num1))
            {
                return false;
            }
            if (Pos == num1)
            {
                Pos--;
            }
            if (this.IsDelimiter(String, Pos))
            {
                Left = Pos;
                while ((Left > 0) && this.IsDelimiter(String, (int) (((int) Left) - 1)))
                {
                    Left -= 1;
                }
                Right = Pos;
                while ((Right < (num1 - 1)) && this.IsDelimiter(String, (int) (((int) Right) + 1)))
                {
                    Right += 1;
                }
            }
            else
            {
                Left = Pos;
                while ((Left > 0) && !this.IsDelimiter(String, (int) (((int) Left) - 1)))
                {
                    Left -= 1;
                }
                Right = Pos;
                while ((Right < (num1 - 1)) && !this.IsDelimiter(String, (int) (((int) Right) + 1)))
                {
                    Right += 1;
                }
            }
            return true;
        }

        public int IndexOf(string value)
        {
            for (int num1 = 0; num1 < this.Count; num1++)
            {
                if (((StrItem) this.list[num1]).String.Equals(value))
                {
                    return num1;
                }
            }
            return -1;
        }

        private void InitDelimiters(char[] Delims)
        {
            this.delimiters = new char[(((Delims != null) ? Delims.Length : 0) + 0x20) + 1];
            for (int num1 = 0; num1 <= 0x20; num1++)
            {
                this.delimiters[num1] = (char) ((ushort) num1);
            }
            if (Delims != null)
            {
                for (int num2 = 0; num2 < Delims.Length; num2++)
                {
                    this.delimiters[(0x20 + num2) + 1] = Delims[num2];
                }
            }
            this.UpdateDelimTable();
        }

        public void Insert(int index, string value)
        {
            this.list.Insert(index, this.CreateStrItem(value));
            this.Changed(index, 0x7fffffff);
        }

        public bool IsDelimiter(char ch)
        {
            return this.delimTable.ContainsKey(ch);
        }

        public bool IsDelimiter(int Index, int Pos)
        {
            return this.IsDelimiter(this[Index], Pos);
        }

        public bool IsDelimiter(string String, int Pos)
        {
            return this.delimTable.ContainsKey(String[Pos]);
        }

        public static bool IsDelimiter(Hashtable DelimTable, string String, int Pos)
        {
            return DelimTable.ContainsKey(String[Pos]);
        }

        private static bool IsWholeWord(Hashtable DelimTable, string String, int Start, int Len, ref int WordStart, ref int WordEnd)
        {
            WordStart = Start;
            WordEnd = Start;
            int num1 = String.Length;
            while ((WordStart > 0) && !SyntaxStrings.IsDelimiter(DelimTable, String, WordStart - 1))
            {
                WordStart -= 1;
            }
            while ((WordEnd < (num1 - 1)) && !SyntaxStrings.IsDelimiter(DelimTable, String, WordEnd + 1))
            {
                WordEnd += 1;
            }
            if (WordStart == Start)
            {
                return (Len == ((WordEnd - WordStart) + 1));
            }
            return false;
        }

        public void LoadFile(string FileName)
        {
            this.LoadFile(FileName, null);
        }

        public void LoadFile(string FileName, Encoding Encoding)
        {
            try
            {
                Stream stream1 = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    StreamReader reader1 = (Encoding != null) ? new StreamReader(stream1, Encoding) : new StreamReader(stream1);
                    try
                    {
                        this.LoadStream(reader1);
                    }
                    finally
                    {
                        reader1.Close();
                    }
                }
                finally
                {
                    stream1.Close();
                }
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void LoadStream(TextReader Reader)
        {
            this.BeginUpdate();
            try
            {
                string text1;
                this.Clear();
                while ((text1 = Reader.ReadLine()) != null)
                {
                    this.Add(text1);
                }
                if (this.source != null)
                {
                    this.source.State |= NotifyState.CountChanged;
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public bool MoveNext()
        {
            this.currentIndex++;
            return (this.currentIndex < this.Count);
        }

        public void Notify()
        {
            this.StringsChanged();
        }

        public int PosToTabPos(string String, int Pos)
        {
            return this.PosToTabPos(String, Pos, false);
        }

        protected internal int PosToTabPos(string String, int Pos, bool TabEnd)
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            string[] textArray1 = String.Split(this.tabArray);
            for (int num5 = 0; num5 < (textArray1.Length - 1); num5++)
            {
                int num4 = textArray1[num5].Length;
                num2 = (this.GetTabStop(num1 + num4) - num1) - num4;
                num1 += (num4 + num2);
                if (Pos < num1)
                {
                    if (((num1 - Pos) < num2) && !TabEnd)
                    {
                        num3 += (num2 - (num1 - Pos));
                    }
                    break;
                }
                num3 += (num2 - 1);
            }
            return (Pos - num3);
        }

        public void Remove(string value)
        {
            int num1 = this.IndexOf(value);
            if (num1 >= 0)
            {
                this.RemoveAt(num1);
            }
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < this.Count))
            {
                this.list.RemoveAt(index);
                if (index < this.Count)
                {
                    StrItem item1 = (StrItem) this.list[index];
					unchecked
					{
						item1.State &= ((StrItemState)(-2));
					}
                }
                this.Changed(index, 0x7fffffff);
            }
        }

        public void RemoveNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Remove(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public void Reset()
        {
            this.currentIndex = 0;
        }

        public void ResetDelimiters()
        {
            this.Delimiters = EditConsts.DefaultDelimiters.ToCharArray();
        }

        public virtual void ResetTabStops()
        {
            int[] numArray1 = new int[1] { EditConsts.DefaultTabStop } ;
            this.TabStops = numArray1;
        }

        public virtual void ResetUseSpaces()
        {
            this.UseSpaces = false;
        }

        public void SaveFile(string FileName)
        {
            this.SaveFile(FileName, null);
        }

        public void SaveFile(string FileName, Encoding Encoding)
        {
            try
            {
                Stream stream1 = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    StreamWriter writer1 = (Encoding != null) ? new StreamWriter(stream1, Encoding) : new StreamWriter(stream1);
                    try
                    {
                        this.SaveStream(writer1);
                    }
                    finally
                    {
                        writer1.Close();
                    }
                }
                finally
                {
                    stream1.Close();
                }
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void SaveStream(TextWriter Writer)
        {
            foreach (StrItem item1 in this.list)
            {
                Writer.WriteLine(item1.String);
            }
        }

        protected void StringsChanged()
        {
            if (this.updateCount == 0)
            {
                if (this.notifyHandler != null)
                {
                    this.notifyHandler(this, EventArgs.Empty);
                }
                this.firstChanged = -1;
                this.lastChanged = -1;
            }
        }

        public int TabPosToPos(string String, int Pos)
        {
            int num1 = Pos - String.Length;
            Pos = this.GetTabString(String.Substring(0, Math.Min(Pos, String.Length))).Length;
            if (num1 > 0)
            {
                return (Pos + num1);
            }
            return Pos;
        }

        public IList ToArrayList()
        {
            ArrayList list1 = new ArrayList(this.Count);
            foreach (StrItem item1 in this.list)
            {
                list1.Add(item1.String);
            }
            return list1;
        }

        public string[] ToStringArray()
        {
            string[] textArray1 = new string[this.Count];
            for (int num1 = 0; num1 < this.Count; num1++)
            {
                textArray1[num1] = this[num1];
            }
            return textArray1;
        }

        private void UpdateDelimTable()
        {
            this.delimTable.Clear();
            char[] chArray1 = this.delimiters;
            for (int num1 = 0; num1 < chArray1.Length; num1++)
            {
                char ch1 = chArray1[num1];
                this.delimTable.Add(ch1, ch1);
            }
        }


        // Properties
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public object Current
        {
            get
            {
                if ((this.currentIndex >= 0) && (this.currentIndex < this.Count))
                {
                    return ((StrItem) this.list[this.currentIndex]).String;
                }
                return null;
            }
        }

        public char[] Delimiters
        {
            get
            {
                return this.delimiters;
            }
            set
            {
                this.delimiters = new char[value.Length];
                Array.Copy(value, this.delimiters, value.Length);
                this.UpdateDelimTable();
            }
        }

        public string DelimiterString
        {
            get
            {
                string text1 = string.Empty;
                char[] chArray1 = this.Delimiters;
                for (int num1 = 0; num1 < chArray1.Length; num1++)
                {
                    char ch1 = chArray1[num1];
                    if (ch1 > ' ')
                    {
                        text1 = text1 + ch1;
                    }
                }
                return text1;
            }
            set
            {
                this.InitDelimiters(value.ToCharArray());
            }
        }

        public Hashtable DelimTable
        {
            get
            {
                return this.delimTable;
            }
        }

        public int FirstChanged
        {
            get
            {
                return this.firstChanged;
            }
            set
            {
                this.firstChanged = value;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this.list.IsSynchronized;
            }
        }

        public string this[int index]
        {
            get
            {
                if ((index >= 0) && (index < this.Count))
                {
                    return ((StrItem) this.list[index]).String;
                }
                return string.Empty;
            }
            set
            {
                StrItem item1 = (StrItem) this.list[index];
                item1.String = value;
				unchecked { item1.State &= ((StrItemState)(-2)); }
                this.Changed(index);
            }
        }

        public int LastChanged
        {
            get
            {
                return this.lastChanged;
            }
            set
            {
                this.lastChanged = value;
            }
        }

        public ITextSource Source
        {
            get
            {
                return this.source;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.list.SyncRoot;
            }
        }

        public int[] TabStops
        {
            get
            {
                return this.tabStops;
            }
            set
            {
                this.tabStops = new int[value.Length];
                Array.Copy(value, this.tabStops, value.Length);
                this.tabList.Clear();
                int num1 = 0;
                int[] numArray1 = this.tabStops;
                for (int num3 = 0; num3 < numArray1.Length; num3++)
                {
                    int num2 = numArray1[num3];
                    if (num2 <= num1)
                    {
                        ErrorHandler.Error(new Exception(string.Format(EditConsts.InvalidTabStop, value.ToString())));
                    }
                    num1 = num2;
                }
            }
        }

        public string Text
        {
            get
            {
                StringBuilder builder1 = new StringBuilder();
                foreach (StrItem item1 in this.list)
                {
                    builder1.Append(item1.String + Consts.CRLF);
                }
                if (builder1.Length >= 2)
                {
                    builder1.Remove(builder1.Length - 2, 2);
                }
                return builder1.ToString();
            }
            set
            {
                if (this.source != null)
                {
                    this.source.BeginUpdate(UpdateReason.Other);
                }
                this.BeginUpdate();
                try
                {
                    this.Clear();
                    if ((value != null) && (value != string.Empty))
                    {
                        string text1;
                        StringReader reader1 = new StringReader(value);
                        while ((text1 = reader1.ReadLine()) != null)
                        {
                            this.Add(text1);
                        }
                    }
                    if (this.source != null)
                    {
                        this.source.State |= NotifyState.CountChanged;
                    }
                }
                finally
                {
                    this.EndUpdate();
                    if (this.source != null)
                    {
                        this.source.EndUpdate();
                    }
                }
            }
        }

        public int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        public bool UseSpaces
        {
            get
            {
                return this.useSpaces;
            }
            set
            {
                this.useSpaces = value;
            }
        }

        public object XmlInfo
        {
            get
            {
                return new XmlSyntaxStringsInfo(this);
            }
            set
            {
                ((XmlSyntaxStringsInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private int currentIndex;
        private char[] delimiters;
        private Hashtable delimTable;
        private int firstChanged;
        private int lastChanged;
        private ArrayList list;
        private TextSource source;
        private char[] tabArray;
        private StringBuilder tabBuilder;
        private ArrayList tabList;
        private int[] tabStops;
        private int updateCount;
        private bool useSpaces;
    }
}

