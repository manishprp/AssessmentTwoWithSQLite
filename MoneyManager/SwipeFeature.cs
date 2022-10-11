﻿using Android.App;
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
using System.Collections.Generic;
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
        public override bool OnMove(RecyclerView p0, RecyclerView.ViewHolder p1, RecyclerView.ViewHolder p2)
        {
            throw new NotImplementedException();
        }
        public override void OnSwiped(RecyclerView.ViewHolder p0, int p1)
        {
            var fam = DataBaseClass.GetFamily();
            var position = p0.LayoutPosition;
            var famToDelete = fam[position];
            switch(p1)
            {
                case ItemTouchHelper.Left:
                   
                    swipeDone?.Invoke(this, new MessageSender { swipeEnums = SwipeEnums.Deleted, position =position, familyRecord=famToDelete });
                    break;
                case ItemTouchHelper.Right:
                    swipeDone?.Invoke(this, new MessageSender { swipeEnums = SwipeEnums.Updated,position=position, familyRecord = famToDelete });
                    break;
            }
        }
    }

    public class MessageSender : EventArgs
    {
        public SwipeEnums swipeEnums;
        public int position;
        public FamilyRecord familyRecord;
    }
}