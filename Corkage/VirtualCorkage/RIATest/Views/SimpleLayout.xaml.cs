using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using RIATest.Web;
using System.ServiceModel.DomainServices.Client;
using RIATest.Web.Models;

namespace RIATest.Views
{
    public partial class SimpleLayout : Page
    {
        public SimpleLayout()
        {
            InitializeComponent();

            // the page is cached, but is discarded when the Frame's cache limit is reached
            this.NavigationCacheMode = NavigationCacheMode.Enabled; 
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CorkageDomainContext cxt = new CorkageDomainContext();
            EntityQuery<CategoryTaskPresentationModel> query3 = cxt.GetCategorisedTasksQuery(1);
            cxt.Load(query3, callBack, cxt);

            //EntityQuery<StoryPM> query3 = cxt.GetStoryDetailsQuery(1);
            //cxt.Load(query3, callBack, cxt);

            //EntityQuery<Sprint> query3 = cxt.GetSprintsQuery();
            //cxt.Load(query3, callBack, cxt);
        }


        //private void callBack(LoadOperation<StoryPM> obj)
        //{
        //    var list = obj.Entities.ToList();
        //    lstCategories.ItemsSource = list[1].CategoryTasks ;
        //}

        private void callBack(LoadOperation<CategoryTaskPresentationModel> obj)
        {
            var list = obj.Entities.ToList();
            lstCategories.ItemsSource = list;
        }

        #region Old Code
        //// Executes when the user navigates to this page.
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    //TaskContext context = new TaskContext();

        //    //EntityQuery<TaskPresentationModel> query1 =
        //    //    from task in context.GetTasksQuery()
        //    //    where task.StoryId == 1
        //    //    select task;

        //    //EntityQuery<CategoryTaskPresentationModel> query2 =
        //    //   from task in context.GetCategoryTasksQuery()
        //    //   where task.StoryId == 1
        //    //   select task;

        //    //context.Load<CategoryTaskPresentationModel>(query2);
        //    //TaskGrid.ItemsSource = context.CategoryTaskPresentationModels;

        //    CorkageDomainContext cxt = new CorkageDomainContext();
        //    EntityQuery<CategoryTaskPresentationModel> query3 = cxt.GetCategorisedTasksQuery(1);
        //    cxt.Load(query3, callBack, cxt);
        //}

        //private void callBack(LoadOperation<CategoryTaskPresentationModel> obj)
        //{
        //    var list = obj.Entities.ToList();
        //    lstCategories.ItemsSource = list;
        //    //foreach (Story s in list)
        //    //{
        //    //    MyControlLibrary.TaskCard card = new MyControlLibrary.TaskCard()
        //    //    {
        //    //        Name = s.StoryId.ToString(),
        //    //        Height = 90,
        //    //        Width = 150,
        //    //        Text = s.Description
        //    //    };

        //    //    //not bound, just pushed onto the control....
        //    //    //look at http://stackoverflow.com/questions/507535/silverlight-itemscontrol-can-you-remove-the-panel-completely-via-templating
        //    //    myGrid.Children.Add(card);
        //    //}
        //} 
        #endregion

    }
}
