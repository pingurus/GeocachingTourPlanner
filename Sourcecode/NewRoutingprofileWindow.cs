﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	public partial class NewRoutingprofileWindow : Form
	{
		public NewRoutingprofileWindow()
		{
			InitializeComponent();
		}

		public NewRoutingprofileWindow(Routingprofile RoutingProfileToEdit)
		{
			try
			{
				InitializeComponent();
				NameValue.Text = RoutingProfileToEdit.Name;

				//Distance
				MaxDistance.Text = RoutingProfileToEdit.MaxDistance.ToString();
				PenaltyPerExtraKM.Text = RoutingProfileToEdit.PenaltyPerExtraKM.ToString();

				//Time
				MaxTime.Text = RoutingProfileToEdit.MaxTime.ToString();
				PenaltyPerExtraKM.Text = RoutingProfileToEdit.PenaltyPerExtra10min.ToString();
				TimePerGeocache.Text = RoutingProfileToEdit.TimePerGeocache.ToString();

				//Profile
				VehicleValue.Text = RoutingProfileToEdit.ItineroProfile.profile.Name;
				switch (RoutingProfileToEdit.ItineroProfile.profile.Metric)
				{
					case Itinero.Profiles.ProfileMetric.DistanceInMeters:

						ModeValue.Text = "Distance";
						break;

					case Itinero.Profiles.ProfileMetric.TimeInSeconds:
						ModeValue.SelectedText = "Time";
						break;
				}
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Couldn't load the complete profile.", "Warning");
			}
		}

		private void CancelNewProfileButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CreateButton_Click(object sender, EventArgs e)
		{
			Routingprofile Profile = new Routingprofile();
			if (NameValue.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			try
			{
				Profile.Name = NameValue.Text;

				Profile.MaxDistance = int.Parse(MaxDistance.Text);
				Profile.PenaltyPerExtraKM = int.Parse(PenaltyPerExtraKM.Text);

				Profile.MaxTime = int.Parse(MaxTime.Text);
				Profile.PenaltyPerExtra10min = int.Parse(PenaltyPerExtra10min.Text);
				Profile.TimePerGeocache = int.Parse(TimePerGeocache.Text);
				
				Profile.ItineroProfile = new SerializableItineroProfile(VehicleValue.Text,ModeValue.Text);
				
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Please fill all fields");
				return;
			}
			catch (FormatException)
			{
				MessageBox.Show("Some fields are filled with incompatible values","Error");
				return;
			}

			//Eintragen des neuen Profils
			foreach (Routingprofile BP in Program.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				Program.Routingprofiles.Remove(BP);
			}
			Program.Routingprofiles.Add(Profile);
			//The Dropdownmenu is updated via an event handler
			if (Program.Backup(Program.Routingprofiles))
			{
				Close();//Only close window if backup was successfull
			}
		}

		private void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ComboBox)sender).Text = ((ComboBox)sender).SelectedItem.ToString();//So I can just check the text and it doesn't matter whether the user typed it or selected it
		}
	}
}
