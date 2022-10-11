using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using MoneyManager.Adapter;
using MoneyManager.Model;
using System;
using System.Collections.Generic;

namespace MoneyManager
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    { 
        private RecyclerView _addFamilyRecyclerView;
        private FloatingActionButton _fab;
        public List<FamilyRecord> _familyRecord = new List<FamilyRecord>();
        private FamilyRecordRecyclerViewAdapter _familyRecordRecyclerViewAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            UiConnection();
            SetUpRecyclerView();
            UiClickEvents();
        }
        protected override void OnResume()
        {
            if(_familyRecordRecyclerViewAdapter==null && FetchDataFromDb()!=null)
            {
                SetUpRecyclerView();
            }
            if(_familyRecordRecyclerViewAdapter!=null)
            CheckForUpdate();
            base.OnResume();
        }

        private void CheckForUpdate()
        {
            _familyRecordRecyclerViewAdapter.UpdateAdapter(FetchDataFromDb());
            if(_familyRecord.Count!=0 && _familyRecord!=null)
            _addFamilyRecyclerView.SmoothScrollToPosition(_familyRecord.Count - 1);
        }

        private void UiClickEvents()
        {
            _fab.Click += _fab_Click;
        }

        private void _fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddFamilyActivity));
            StartActivity(intent);
        }

        private void SetUpRecyclerView()
        {
            _familyRecord.Clear();
            _familyRecord = FetchDataFromDb();
            if(_familyRecord!=null)
            {
                _familyRecordRecyclerViewAdapter = new FamilyRecordRecyclerViewAdapter(_familyRecord);
                _addFamilyRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                _addFamilyRecyclerView.SetAdapter(_familyRecordRecyclerViewAdapter);
                _addFamilyRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
                _familyRecordRecyclerViewAdapter.ItemClick += _familyRecordRecyclerViewAdapter_ItemClick;
                SwipeFeature item = new SwipeFeature(ItemTouchHelper.Up|ItemTouchHelper.Down|ItemTouchHelper.Start|ItemTouchHelper.End, ItemTouchHelper.Left | ItemTouchHelper.Right);
                ItemTouchHelper itemTouch = new ItemTouchHelper(item);
                itemTouch.AttachToRecyclerView(_addFamilyRecyclerView);
                item.swipeDone += Item_swipeDone;
            }
            
        }

        private void Item_swipeDone(object sender, MessageSender e)
        {

           CheckForUpdate();
            if(e.swipeEnums == SwipeEnums.Updated)
            {
                UpdateFamily(e.position);
            }
            if(e.swipeEnums == SwipeEnums.Deleted)
            {
                _familyRecord.RemoveAt(e.position);
                _familyRecordRecyclerViewAdapter.UpdateAdapter(_familyRecord);
                var res = DataBaseClass.DeleteFamily(e.familyRecord);
                    Snackbar.Make(this, _addFamilyRecyclerView, "Deleted", Snackbar.LengthShort).
                    SetAction("Undo", v =>
                    {
                        var id = e.familyRecord.id;
                        DataBaseClass.UpdateFamily(e.familyRecord, id);
                        _familyRecordRecyclerViewAdapter.UpdateAdapter(DataBaseClass.GetFamily());
                    }).Show();
            }
        }

        private void _familyRecordRecyclerViewAdapter_ItemClick(object sender, FamilyRecordRecyclerViewAdapterClickEventArgs e)
        {
            UpdateFamily(e.Position);
        }

        private void UpdateFamily(int position)
        {
            var famObject = _familyRecord[position];
            Intent intent = new Intent(this, typeof(AddFamilyActivity));
            intent.PutExtra("id", famObject.id);
            intent.AddFlags(ActivityFlags.ClearTop);
            StartActivity(intent);
        }

        private List<FamilyRecord> FetchDataFromDb()
        {

            _familyRecord = DataBaseClass.GetFamily();
            return _familyRecord;
        }

        private void UiConnection()
        {
            _addFamilyRecyclerView = FindViewById<RecyclerView>(Resource.Id.familyRecyclerView);
            _fab = FindViewById<FloatingActionButton>(Resource.Id.addFamily);
        }
    }
}