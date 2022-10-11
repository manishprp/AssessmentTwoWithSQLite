using Android.Text;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.TextField;
using Java.Interop;
using Java.Lang;
using MoneyManager.Model;
using System;
using System.Collections.Generic;

namespace MoneyManager.Adapter
{
    public class AddChildRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<AddChildRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<AddChildRecyclerViewAdapterClickEventArgs> ItemLongClick;
        public List<Children> items;
        public AddChildRecyclerViewAdapter(List<Children> data)
        {
            items = data;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = null;
            var id = Resource.Layout.addchild_rowitem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
            var vh = new AddChildRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];
            var holder = viewHolder as AddChildRecyclerViewAdapterViewHolder;
            if (item.ChildName !="")
            {
                holder._childName.EditText.Text = item.ChildName;
            }
        }
        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }
        public override int ItemCount => items.Count;

        void OnClick(AddChildRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(AddChildRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class AddChildRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextInputLayout _childName { get; set; }
        public AddChildRecyclerViewAdapterViewHolder(View itemView, Action<AddChildRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<AddChildRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            _childName = itemView.FindViewById<TextInputLayout>(Resource.Id.enterChildEditTExt);
            itemView.Click += (sender, e) => clickListener(new AddChildRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new AddChildRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
    

    public class AddChildRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}