using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.DomainServices.Server;
using RIATest.Web.Models;
using System.ServiceModel.DomainServices.Hosting;

namespace RIATest.Web.Services
{
    /// <summary>
    /// Responsibility is to handle queries for managing tasks
    /// </summary>
    [EnableClientAccess]
    public class TaskService : DomainService
    {
        private CorkageEntities _context = new CorkageEntities();

        //public IQueryable<TaskPresentationModel> GetTasks()
        //{
        //    return from task in _context.Tasks
        //           select new TaskPresentationModel()
        //           {
        //               TaskId = task.TaskId,
        //               StoryId = task.StoryId,
        //               Description = task.Description
        //           };
        //}

        //public IQueryable<CategoryTaskPresentationModel> GetCategoryTasks()
        //{
        //    return from task in _context.Tasks
        //           select new CategoryTaskPresentationModel()
        //           {
        //               TaskId = task.TaskId,
        //               StoryId = task.StoryId,
        //               CategoryId = task.CategoryId,
        //               Category = task.Category.Description, 
        //               Description = task.Description
        //           };
        //}
    }
}