using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GDMultiStash.Common.Overlay
{
    
    public class ListBoxItemElement<T> : Element
    {
        public virtual int Order { get; set; } = 0;

        public T Model { get; set; }

        public ListBoxItemElement()
        {
            MouseCheckChildren = false;
            WidthToParent = true;
        }
    }

    public class ListBoxElement<T, U> : Element, IScrollable where T : ListBoxItemElement<U>
    {

        public virtual float ItemHeight => 0f;
        public virtual float ItemMargin => 0f;

        private readonly List<T> _scrollItems;
        //private readonly List<T> _cache = new List<T>();

        private bool _updateList = false;

        public IEnumerable<T> ScrollItems => _scrollItems.AsReadOnly();

        public ScrollHandler ScrollHandler { get; private set; }

        public override float Height => base.Height; // disable setter

        public ListBoxElement()
        {
            MouseCheckNeedBaseHit = true;
            _scrollItems = new List<T>();
            ScrollHandler = new ScrollHandler(ScrollOrientation.Vertical);
            ScrollHandler.VisibleUnitsYChanged += delegate {
                base.Height = ScrollHandler.VisibleUnitsY * (ItemHeight + ItemMargin) - ItemMargin;
                _updateList = true;
            };
            ScrollHandler.ScrollPositionYChanged += delegate {
                _updateList = true;
            };
        }
        /*
        protected T GetCachedScrollItem()
        {
            if (_cache.Count == 0) return null;
            T t = _cache[0];
            _cache.RemoveAt(0);
            return t;
        }
        */
        public virtual void AddScrollItem(T item)
        {
            item.Height = ItemHeight;
            _scrollItems.Add(item);
            AddChild(item);

            ScrollHandler.TotalUnitsY = _scrollItems.Count;
            _updateList = true;
        }

        public virtual void AddScrollItems(IEnumerable<T> items)
        {
            foreach (T item in items)
                AddScrollItem(item);

            ScrollHandler.TotalUnitsY = _scrollItems.Count;
            _updateList = true;
        }

        public virtual void RemoveScrollItem(T child)
        {
            //_cache.Add(child);
            child.Destroy();
            _scrollItems.Remove(child);

            ScrollHandler.TotalUnitsY = _scrollItems.Count;
            _updateList = true;
        }

        public virtual void ClearScrollItems()
        {
            foreach(T item in _scrollItems) {
                item.Destroy();
                //_cache.Add(item);
            }
            _scrollItems.Clear();

            ScrollHandler.TotalUnitsY = 0;
            _updateList = true;
        }

        public virtual void SetScrollItems(IEnumerable<T> items)
        {
            ClearScrollItems();
            AddScrollItems(items);

            ScrollHandler.TotalUnitsY = _scrollItems.Count;
            _updateList = true;
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            if (_updateList)
            {
                _updateList = false;

                int startIndex = ScrollHandler.ScrollPositionY;
                int endIndex = startIndex + ScrollHandler.VisibleUnitsY - 1;
                float baseY = -ScrollHandler.ScrollPositionY * (ItemHeight + ItemMargin);

                _scrollItems.Sort((a, b) => a.Order.CompareTo(b.Order));

                int i = 0;
                foreach (T t in _scrollItems)
                {
                    //if (!t.Visible) continue;
                    t.X = 0;
                    t.Y = baseY + i * (ItemHeight + ItemMargin);
                    t.Visible = i >= startIndex && i <= endIndex;
                    i += 1;
                }

                Redraw();
            }
        }

    }
}
