using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MoneyManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Adapter
{
    public class FamilyRecordRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<FamilyRecordRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<FamilyRecordRecyclerViewAdapterClickEventArgs> ItemLongClick;
        List<FamilyRecord> items;

        public FamilyRecordRecyclerViewAdapter(List<FamilyRecord> data)
        {
            items = data;
        }
   
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.family_row_item;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new FamilyRecordRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        public void UpdateAdapter(List<FamilyRecord> _familyRecordIn)
        {
            items.Clear();
            items.AddRange(_familyRecordIn);
            NotifyDataSetChanged();
        }


        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as FamilyRecordRecyclerViewAdapterViewHolder;
            holder.fatherNameTextView.Text = item.FatherName;
            holder.motherNameTextView.Text = item.MotherName;
            holder.addressTextView.Text = item.Address;
            List<string> strings = new List<string>();
            for(int i=0;i<item.Child.Count;i++)
            {
                strings.Add(item.Child[i].ChildName);
            }

            holder.childrenNamesTextView.Text = String.Join(", ",strings);
            //holder.TextView.Text = items[position];
        }
            public override int ItemCount => items.Count;

        void OnClick(FamilyRecordRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(FamilyRecordRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }


    public class FamilyRecordRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView fatherNameTextView { get; set; }
        public TextView motherNameTextView { get; set; }
        public TextView addressTextView { get; set; }
        public TextView childrenNamesTextView { get; set; }


        public FamilyRecordRecyclerViewAdapterViewHolder(View itemView, Action<FamilyRecordRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<FamilyRecordRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            fatherNameTextView = itemView.FindViewById<TextView>(Resource.Id.fatherNameTextView);
            motherNameTextView = itemView.FindViewById<TextView>(Resource.Id.motherNameTextView);
            addressTextView = itemView.FindViewById<TextView>(Resource.Id.addressTextView);
            childrenNamesTextView = itemView.FindViewById<TextView>(Resource.Id.childrenNamesTextView);
            itemView.Click += (sender, e) => clickListener(new FamilyRecordRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new FamilyRecordRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class FamilyRecordRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}