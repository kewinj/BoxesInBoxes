using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using MyControlLibrary;
using RIATest.Web;
using Page = System.Windows.Controls.Page;

namespace RIATest.Views
{
    public partial class Corkage : Page
    {
        public Corkage()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //CorkageDomainContext c = new CorkageDomainContext();
            //EntityQuery<Story> query = c.GetStoriesQuery();
            //c.Load(query, StoriesCallBack, c );

            //var tasksQuery = c.GetTasksQuery();
            //c.Load(tasksQuery, TasksCallBack, c);


        }

        private void storyDomainDataSource_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            //var list = obj.Entities.ToList();
            //myItem.ItemsSource = list;
            //foreach (Story s in list)
            //{
            //    MyControlLibrary.TaskCard card = new MyControlLibrary.TaskCard()
            //    {
            //        Name = s.StoryId.ToString(),
            //        Height = 90,
            //        Width = 150,
            //        Text = s.Description
            //    };
            //    //not bound, just pushed onto the control....
            //    //look at http://stackoverflow.com/questions/507535/silverlight-itemscontrol-can-you-remove-the-panel-completely-via-templating
            //    myGrid.Children.Add(card);
            //}

            if(e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
    }
}
