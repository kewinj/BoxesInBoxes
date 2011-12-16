
namespace RIATest.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
using RIATest.Web.Models;


    // Implements application logic using the CorkageEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class CorkageDomainService : LinqToEntitiesDomainService<CorkageEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Categories' query.
        [Query(IsDefault = true)]
        public IQueryable<Category> GetCategories()
        {
            return this.ObjectContext.Categories;
        }

        public void InsertCategory(Category category)
        {
            if((category.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(category, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Categories.AddObject(category);
            }
        }

        public void UpdateCategory(Category currentCategory)
        {
            this.ObjectContext.Categories.AttachAsModified(currentCategory, this.ChangeSet.GetOriginal(currentCategory));
        }

        public void DeleteCategory(Category category)
        {
            if((category.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(category, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Categories.Attach(category);
                this.ObjectContext.Categories.DeleteObject(category);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Sprints' query.
        [Query(IsDefault = true)]
        public IQueryable<Sprint> GetSprints()
        {
            this.ObjectContext.ContextOptions.LazyLoadingEnabled = false;

            return this.ObjectContext.Sprints;
        }

        public void InsertSprint(Sprint sprint)
        {
            if((sprint.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprint, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Sprints.AddObject(sprint);
            }
        }

        public void UpdateSprint(Sprint currentSprint)
        {
            this.ObjectContext.Sprints.AttachAsModified(currentSprint, this.ChangeSet.GetOriginal(currentSprint));
        }

        public void DeleteSprint(Sprint sprint)
        {
            if((sprint.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprint, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Sprints.Attach(sprint);
                this.ObjectContext.Sprints.DeleteObject(sprint);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Stories' query.
        [Query(IsDefault = true)]
        public IQueryable<Story> GetStories()
        {

            return this.ObjectContext.Stories.Include("Tasks");
        }

        public void InsertStory(Story story)
        {
            if((story.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(story, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Stories.AddObject(story);
            }
        }

        public void UpdateStory(Story currentStory)
        {
            this.ObjectContext.Stories.AttachAsModified(currentStory, this.ChangeSet.GetOriginal(currentStory));
        }

        public void DeleteStory(Story story)
        {
            if((story.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(story, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Stories.Attach(story);
                this.ObjectContext.Stories.DeleteObject(story);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Tasks' query.
        [Query(IsDefault = true)]
        public IQueryable<Task> GetTasks()
        {
            return this.ObjectContext.Tasks;
        }

        public void InsertTask(Task task)
        {
            if((task.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(task, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Tasks.AddObject(task);
            }
        }

        public void UpdateTask(Task currentTask)
        {
            this.ObjectContext.Tasks.AttachAsModified(currentTask, this.ChangeSet.GetOriginal(currentTask));
        }

        public void DeleteTask(Task task)
        {
            if((task.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(task, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Tasks.Attach(task);
                this.ObjectContext.Tasks.DeleteObject(task);
            }
        }


        public IQueryable<CategoryTaskPresentationModel> GetCategorisedTasks(int storyId)
        {
            return
                   from category in this.ObjectContext.Categories 
                   select new CategoryTaskPresentationModel()
                   {
                       CategoryId = category.CategoryId,
                       Category = category.Description,
                       StoryId = storyId, 
                       Tasks = 
                        from task in this.ObjectContext.Tasks
                        where task.StoryId == storyId 
                        && task.CategoryId == category.CategoryId 
                        select new TaskPresentationModel ()
                        {
                            TaskId = task.TaskId,
                            StoryId = task.StoryId,
                            CategoryId = task.CategoryId,
                            Description = task.Description 
                        }
                   };
        }

        public IEnumerable<StoryPM> GetStoryDetails(int sprintId)
        {

            var stories = from story in this.ObjectContext.Stories

                   select new StoryPM()
                   {
                       StoryId = story.StoryId,
                       SprintId = 1,
                       Description = story.Description,
                       //CategoryTasks = (from category in this.ObjectContext.Categories
                       //                select new CategoryTaskPresentationModel()
                       //                {
                       //                    CategoryId = category.CategoryId,
                       //                    Category = category.Description,
                       //                    StoryId = story.StoryId
                       //                }) 
                   };

            var stories2 = stories.ToList();

            for(int i=0; i<stories2.Count(); i++)
            {
                stories2[i].CategoryTasks = GetCategorisedTasks(stories2[i].StoryId );  
            }

            return stories2;
        }

            //return
            //    (from story in this.ObjectContext.Stories
            //    select new StoryPM()
            //    {
            //        StoryId = story.StoryId,
            //        SprintId = 1,
            //        Description = story.Description,
            //        CategoryTasks = 
            //                        from category in this.ObjectContext.Categories
            //                        where category.Tasks.Count > 0 
            //                        select new CategoryTaskPresentationModel()
            //                        {
            //                            CategoryId = category.CategoryId,
            //                            Category = category.Description,
            //                            StoryId = story.StoryId,
            //                            Tasks =
            //                             from task in this.ObjectContext.Tasks
            //                           //  where task.StoryId == story.StoryId && task.CategoryId == category.CategoryId
            //                             where task.CategoryId == category.CategoryId
            //                             select new TaskPresentationModel()
            //                             {
            //                                 TaskId = task.TaskId,
            //                                 StoryId = task.StoryId,
            //                                 CategoryId = task.CategoryId,
            //                                 Description = task.Description
            //                             }
            //                        }
            //    });

        


    }
}


