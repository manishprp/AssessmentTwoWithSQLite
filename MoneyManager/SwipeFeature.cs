using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MoneyManager.Adapter;
using MoneyManager.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using static Android.Service.Voice.VoiceInteractionSession;

namespace MoneyManager
{
    public class SwipeFeature : ItemTouchHelper.SimpleCallback
    {

        public SwipeFeature(int dragDirs, int swipeDirs) : base(dragDirs, swipeDirs)
        {
        }
        public event EventHandler<MessageSender> swipeDone;
        public event EventHandler<MessageSender> onMoveDone;
        public override bool OnMove(RecyclerView p0, RecyclerView.ViewHolder p1, RecyclerView.ViewHolder p2)
        {
            int fromPosition = p1.LayoutPosition;
            int toPosition = p2.LayoutPosition;
            var fam = DataBaseClass.GetFamily();
            var list = Swap(fam,fromPosition,toPosition);
            p0.GetAdapter().NotifyItemMoved(fromPosition, toPosition);
            DataBaseClass.ReplaceAll(list);
            //onMoveDone?.Invoke(this, new MessageSender { famRecords = list, swipeEnums= SwipeEnums.Moved, position=-1, familyRecord=null});
            return true;
        }

        private List<FamilyRecord> Swap(List<FamilyRecord> fam, int fromPosition, int toPosition)
        {
           var temp = fam[fromPosition];
            fam[fromPosition] = fam[toPosition];
            fam[toPosition] = temp;
            return fam;
        }

        public override void OnSwiped(RecyclerView.ViewHolder p0, int p1)
        {
            var fam = DataBaseClass.GetFamily();
            var position = p0.LayoutPosition;
            var famToDelete = fam[position];
            switch(p1)
            {
                case ItemTouchHelper.Left:
                    swipeDone?.Invoke(this, new MessageSender { swipeEnums = SwipeEnums.Deleted, position =position, familyRecord=famToDelete, famRecords=null });
                    break;
                case ItemTouchHelper.Right:
                    swipeDone?.Invoke(this, new MessageSender { swipeEnums = SwipeEnums.Updated,position=position, familyRecord = famToDelete, famRecords=null });
                    break;
            }
        }
    }

    public class MessageSender : EventArgs
    {
        public SwipeEnums swipeEnums;
        public int position;
        public FamilyRecord familyRecord;
        public List<FamilyRecord> famRecords;
    }
}