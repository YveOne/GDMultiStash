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

        public PseudoScrollChild()
        {
            WidthToParent = true;
        }
    }

    public class PseudoScrollElement<T> : Element where T : PseudoScrollChild
    {
        protected virtual float ItemHeight => 0f;
        protected virtual float ItemMargin => 0f;

        private readonly List<T> _scrollChildren;

        private int _maxVisibleCount = 0;
        private int _scrollIndex = 0;
        private readonly List<T> _cache;

        public PseudoScrollElement()
        {
            _scrollChildren = new List<T>();
            _cache = new List<T>();
        }

        public int ItemCount => _scrollChildren.Count;

        public int Scrollindex
        {
            get => _scrollIndex;
            set {
                int min = _maxVisibleCount - _scrollChildren.Count;
                if (value < min) value = min;
                if (value > 0) value = 0;
                _scrollIndex = value;
                UpdateList();
            }
        }

        public int MaxVisibleCount
        {
            get { return _maxVisibleCount; }
            set {
                _maxVisibleCount = value;
                Redraw();
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
        }

        private bool _updateList = false;

        public virtual void UpdateList()
        {
            _updateList = true;
        }

        public override void Draw(float ms)
        {
            base.Draw(ms);
            if (_updateList)
            {
                _updateList = false;

                if (_scrollChildren.Count <= _maxVisibleCount)
                    _scrollIndex = 0;
                
                int startIndex = -_scrollIndex;
                int endIndex = startIndex + _maxVisibleCount - 1;
                float baseY = _scrollIndex * (ItemHeight + ItemMargin) + ItemMargin;

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
            }
        }






    }
}
