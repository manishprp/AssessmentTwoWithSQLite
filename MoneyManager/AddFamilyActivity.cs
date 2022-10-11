using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Java.Lang;
using Java.Util.Logging;
using MoneyManager.Adapter;
using MoneyManager.Model;
using System;
using System.Collections.Generic;
using static Android.Icu.Text.Transliterator;
using static Android.Renderscripts.ScriptGroup;
using static Android.Views.ViewTreeObserver;

namespace MoneyManager
{
    [Activity(Label = "AddFamilyActivity", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class AddFamilyActivity : AppCompatActivity
    {
        private MaterialButton _addChildButton, _saveButton;
        private TextInputLayout _enterFatherNameEditText, _enterMotherNameEditText, _enterAddressEditText, _enterChildEditTExt;
        private RecyclerView _enterChildRecyclerView;
        private AddChildRecyclerViewAdapter _addChildRecyclerViewAdapter;
        private FamilyRecord _familyRecord = new FamilyRecord();
        private List<Children> _children = new List<Children>();
        private int _id = 0;
        private int _receivedId = -1;
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.add_family_layout);
            UiConnection();
            UiClickEvents();
        }
        protected override void OnResume()
        {
            _receivedId = Intent.GetIntExtra("id", _receivedId);
            if (_receivedId != -1)
            {
                this.Title = "Update";
                _saveButton.Text = "Update";
                FetchDataAndSetUpOnFields();
            }
            base.OnResume();
        }

        private void FetchDataAndSetUpOnFields()
        {
            var family = DataBaseClass.GetFamilyWithId(_receivedId);
            if (family != null)
            {
                _enterFatherNameEditText.EditText.Text = family.FatherName;
                _enterMotherNameEditText.EditText.Text = family.MotherName;
                _enterAddressEditText.EditText.Text = family.Address;
                if (family.Child.Count > 0)
                {
                    for (int i = 0; i < family.Child.Count; i++)
                    {
                        AddNewTextInputFieldWithName(family, i);
                    }
                }

            }
        }
      
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        private void UiClickEvents()
        {
            _saveButton.Click += _saveButton_Click;
            _addChildButton.Click += _addChildButton_Click;
        }

        private void _saveButton_Click(object sender, EventArgs e)
        {
            CollectTheChildrenName();
            StoreFamilyRecordInObject();
            CreateTablesAndInsertData();
            this.Finish();
        }

        private void CreateTablesAndInsertData()
        {
            DataBaseClass.createDatabase<Children>();
            DataBaseClass.createDatabase<FamilyRecord>();
            if(_receivedId==-1)
            {
                var v = DataBaseClass.insertIntoTable(_familyRecord);
                if (v)
                    Toast.MakeText(this, "Hogaya", ToastLength.Short).Show();
            }
            else
            {
                var v = DataBaseClass.UpdateFamily(_familyRecord, _receivedId);
                if (v)
                    Toast.MakeText(this, "Update Hogaya", ToastLength.Short).Show();
            }
           
        }

        private void StoreFamilyRecordInObject()
        {
            _familyRecord.FatherName = _enterFatherNameEditText.EditText.Text;
            _familyRecord.MotherName = _enterMotherNameEditText.EditText.Text;
            _familyRecord.Address = _enterAddressEditText.EditText.Text;
            _familyRecord.Child = _children;
        }
        public void SetChildrenNamesOnViews(List<Children> children)
        {
            if (children.Count > 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    var view = _enterChildRecyclerView.FindViewHolderForLayoutPosition(i);
                    if (view != null)
                    {
                        _enterChildEditTExt = view.ItemView.FindViewById<TextInputLayout>(Resource.Id.enterChildEditTExt);
                        if (_enterChildEditTExt.EditText.Text != "")
                        {
                            _children[i].ChildName = _enterChildEditTExt.EditText.Text;
                        }
                    }
                }
            }
        }

        private void CollectTheChildrenName()
        {
                if (_children.Count > 0)
                {
                    for (int i = 0; i < _children.Count; i++)
                    {
                        var view = _enterChildRecyclerView.FindViewHolderForLayoutPosition(i);
                        if (view != null)
                        {
                            _enterChildEditTExt = view.ItemView.FindViewById<TextInputLayout>(Resource.Id.enterChildEditTExt);
                            if (_enterChildEditTExt.EditText.Text != "")
                            {
                                _children[i].ChildName = _enterChildEditTExt.EditText.Text.Trim();
                            }
                        }
                    }
                }
        }

        private void _addChildButton_Click(object sender, EventArgs e)
        {
            AddNewTextInputField();
        }
        private void AddNewTextInputFieldWithName(FamilyRecord  familyRecord, int pos)
        {
            if (_addChildRecyclerViewAdapter == null)
            {
                _id++;
                _children.Add(new Children {ChildName = familyRecord.Child[pos].ChildName });
                _addChildRecyclerViewAdapter = new AddChildRecyclerViewAdapter(_children);
                _enterChildRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                _enterChildRecyclerView.SetAdapter(_addChildRecyclerViewAdapter);
            }

            else if (IsAnyBoxEmpty())
            {
                Toast.MakeText(this, "Fill in Box First", ToastLength.Short).Show();
            }
            else
            {
                _id++;
                _children.Add(new Children { ChildName = familyRecord.Child[pos].ChildName });
                _addChildRecyclerViewAdapter.NotifyItemInserted(_children.Count - 1);
            }
        }

        private void AddNewTextInputField()
        {
            if (_addChildRecyclerViewAdapter == null)
            {
                _id++;
                _children.Add(new Children {});
                _addChildRecyclerViewAdapter = new AddChildRecyclerViewAdapter(_children);
                _enterChildRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                _enterChildRecyclerView.SetAdapter(_addChildRecyclerViewAdapter);
            }

            else if(IsAnyBoxEmpty())
            {
                Toast.MakeText(this, "Fill in Box First", ToastLength.Short).Show();
            }
            else
            {
                _id++;
                _children.Add(new Children {});
                _addChildRecyclerViewAdapter.NotifyItemInserted(_children.Count - 1);
            }
        }

        private bool IsAnyBoxEmpty()
        {
            if(_children.Count>0)
            {
                for(int i=0;i< _children.Count;i++)
                {
                   var view = _enterChildRecyclerView.FindViewHolderForLayoutPosition(i);
                    if (view != null)
                    {
                        _enterChildEditTExt = view.ItemView.FindViewById<TextInputLayout>(Resource.Id.enterChildEditTExt);
                        if(_enterChildEditTExt.EditText.Text=="")
                            return true;
                    }
                }
            }
            return false;
        }
        private void UiConnection()
        {
            _addChildButton = FindViewById<MaterialButton>(Resource.Id.addChildButton);
            _saveButton = FindViewById<MaterialButton>(Resource.Id.saveButton);
            _enterFatherNameEditText = FindViewById<TextInputLayout>(Resource.Id.enterFatherNameEditText);
            _enterMotherNameEditText = FindViewById<TextInputLayout>(Resource.Id.enterMotherNameEditText);
            _enterAddressEditText = FindViewById<TextInputLayout>(Resource.Id.enterAddressEditText);
            _enterChildRecyclerView = FindViewById<RecyclerView>(Resource.Id.enterChildRecyclerView);
        }
    }
}