using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GDMultiStash.Common.Overlay
{

    public class PseudoScrollChild : Element
    {

        private int _order = 0;
        public virtual int Order { get { return _order; } set { _order = value; } }

        public Object Model { get; private set; }

        public PseudoScrollChild(Object model)
        {
            WidthToParent = true;
            Model = model;
        }
    }

    public class PseudoScrollElement<T> : Element where T : PseudoScrollChild
    {
        protected virtual float ItemHeight => 0f;
        protected virtual float ItemMargin => 0f;
        protected virtual float ItemMarginStart => 0f;

        private readonly List<T> _scrollChildren;

        private int _maxVisibleCount = 0;
        private int _curVisibleCount = 0;
        private int _scrollIndex = 0;
        private readonly List<T> _cache;

        public delegate void VisibleCountChangedEventHandler(int visibleCount);
        public event VisibleCountChangedEventHandler VisibleCountChanged;

        public PseudoScrollElement()
        {
            _scrollChildren = new List<T>();
            _cache = new List<T>();
        }

        public List<T> Items => _scrollChildren;

        public int Scrollindex
        {
            get => _scrollIndex;
            set {
                int min = _maxVisibleCount - _curVisibleCount;
                if (value < min) value = min;
                if (value > 0) value = 0;
                _scrollIndex = value;
                UpdateList();
            }
        }

        public int MaxVisibleCount
        {
            get { return _maxVisibleCount; }
            set
            {
                _maxVisibleCount = value;
                Redraw();
            }
        }

        public int CurrentVisibleCount
        {
            get { return _curVisibleCount; }
            protected set
            {
                _curVisibleCount = value;
            }
        }

        public override void Reset()
        {
            base.Reset();
            UpdateList();
        }

        protected T GetCachedScrollItem()
        {
            if (_cache.Count == 0) return null;
            T t = _cache[0];
            t.Visible = false;
            _cache.RemoveAt(0);
            return t;
        }

        public virtual void AddScrollItem(T child)
        {
            _scrollChildren.Add(child);
            AddChild(child);
            child.Height = ItemHeight;
            _updateList = true;
        }

        public virtual void RemoveScrollItem(T child)
        {
            _scrollChildren.Remove(child);
            RemoveChild(child.ID);
            _updateList = true;
        }

        public virtual void ClearScrollItems()
        {
            foreach (T t in _scrollChildren)
            {
                RemoveChild(t);
                t.Visible = false;
                _cache.Add(t);
            }
            _scrollChildren.Clear();
            _updateList = true;
        }

        private bool _updateList = false;

        public virtual void UpdateList()
        {
            _updateList = true;
        }

        protected virtual void OnUpdateListStart()
        {

        }

        protected virtual void OnUpdateListEnd()
        {

        }

        public override void Draw(float ms)
        {
            base.Draw(ms);
            if (_updateList)
            {
                _updateList = false;
                OnUpdateListStart();

                int visibleCount = 0;
                foreach (var item in Items)
                    if (item.Visible) visibleCount += 1;
                if (CurrentVisibleCount != visibleCount)
                {
                    CurrentVisibleCount = visibleCount;
                    VisibleCountChanged?.Invoke(visibleCount);
                }

                if (_scrollChildren.Count <= _maxVisibleCount)
                    _scrollIndex = 0;
                
                int startIndex = -_scrollIndex;
                int endIndex = startIndex + _maxVisibleCount - 1;
                float baseY = _scrollIndex * (ItemHeight + ItemMargin) + ItemMarginStart;

                _scrollChildren.Sort((a, b) => a.Order.CompareTo(b.Order));

                int i = 0;
                foreach (T t in _scrollChildren)
                {
                    if (!t.Visible) continue;
                    t.X = 0;
                    t.Y = baseY + i * (ItemHeight + ItemMargin);
                    t.Visible = i >= startIndex && i <= endIndex;
                    i += 1;
                }

                OnUpdateListEnd();
            }
        }






    }
}
