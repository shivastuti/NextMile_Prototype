﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NextMile02
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;


    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "db25cc070b46054a1bb81ba561014a17ed")]
	public partial class NextMileDB1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertUserProfileTest1(UserProfileTest1 instance);
    partial void UpdateUserProfileTest1(UserProfileTest1 instance);
    partial void DeleteUserProfileTest1(UserProfileTest1 instance);
    #endregion
		
		public NextMileDB1DataContext() :
        base(global::System.Configuration.ConfigurationManager.ConnectionStrings["NextMileDB"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public NextMileDB1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NextMileDB1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NextMileDB1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NextMileDB1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<UserProfileTest1> UserProfileTest1s
		{
			get
			{
				return this.GetTable<UserProfileTest1>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.UserProfileTest1")]
	public partial class UserProfileTest1 : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _userid;
		
		private string _truckname;
		
		private System.Nullable<int> _preference;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnuseridChanging(string value);
    partial void OnuseridChanged();
    partial void OntrucknameChanging(string value);
    partial void OntrucknameChanged();
    partial void OnpreferenceChanging(System.Nullable<int> value);
    partial void OnpreferenceChanged();
    #endregion
		
		public UserProfileTest1()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_userid", DbType="NVarChar(60) NOT NULL", CanBeNull=false)]
		public string userid
		{
			get
			{
				return this._userid;
			}
			set
			{
				if ((this._userid != value))
				{
					this.OnuseridChanging(value);
					this.SendPropertyChanging();
					this._userid = value;
					this.SendPropertyChanged("userid");
					this.OnuseridChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_truckname", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string truckname
		{
			get
			{
				return this._truckname;
			}
			set
			{
				if ((this._truckname != value))
				{
					this.OntrucknameChanging(value);
					this.SendPropertyChanging();
					this._truckname = value;
					this.SendPropertyChanged("truckname");
					this.OntrucknameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_preference", DbType="Int")]
		public System.Nullable<int> preference
		{
			get
			{
				return this._preference;
			}
			set
			{
				if ((this._preference != value))
				{
					this.OnpreferenceChanging(value);
					this.SendPropertyChanging();
					this._preference = value;
					this.SendPropertyChanged("preference");
					this.OnpreferenceChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
